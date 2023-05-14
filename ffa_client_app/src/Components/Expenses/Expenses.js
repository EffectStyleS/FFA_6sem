import React, { useState, useEffect } from "react";
import { Table, Button, Modal, Form, Input, DatePicker, Select } from "antd";
import moment from "moment/moment";

import "./Expenses.css"

const Expenses = ({ userId, userRole }) => {
    const [expenses, setExpenses] = useState([]);
    const [selectedExpense, setSelectedExpense] = useState(null);

    const [modalVisible, setModalVisible] = useState(false);
    const [reportVisible, setReportVisible] = useState(false);
    const [reportNullVisible, setReportNullVisible] = useState(false);

    const [expenseTypes, setExpenseTypes] = useState([]);
    const [selectedExpenseType, setSelectedExpenseType] = useState(null);

    const [budgets, setBudgets] = useState([]);

    const [filteredExpensesData, setFilteredExpensesData] = useState(null);
    const [isFilteredExpensesDataEditing, setIsFilteredExpensesDataEditing] = useState(false);
    const [isFilteredExpensesDataSets, setIsFilteredExpensesDataSets] = useState(false);

    const [users, setUsers] = useState(null);
    const [selectedUser, setSelectedUser] = useState(null);

    const [form] = Form.useForm();

    useEffect(() => {

        const fetchUsers = async () => {
            try {
                const usersResponse = await fetch(`api/User`);
                if (usersResponse.status === 200) {
                    const usersData = await usersResponse.json();
                    const formattedUsers = usersData.map(user => ({
                        userId: user.id,
                        userName: user.userName,
                        userRole: user.role
                    }));
                    setUsers(formattedUsers);
    
                    // Установить выбранного пользователя, если userId совпадает
                    const currentUser = formattedUsers.find(user => user.userId === userId);
                    setSelectedUser(currentUser);
                } else {
                    throw new Error("Failed to fetch users");
                }
            } catch (error) {
                console.log(error);
            }
        };
      
        fetchUsers();
      }, []); 

    useEffect(() => {
        const fetchBudgets = async () => {
            try {
                const budgetsResponse = await fetch(`api/Budget/user/${selectedUser.userId}`);
                if (budgetsResponse.status === 200) {
                    const budgetsData = await budgetsResponse.json();    

                    const budgets = budgetsData.map((budget) => {
                        const responseDate = new Date(budget.startDate);
    
                        return {
                            ...budget,
                            startDate: `${responseDate.getFullYear()}-${responseDate.getMonth() + 1}-${responseDate.getDate()}`
                        };
                    });
    
    
                    setBudgets(budgets);
                } else {
                    throw new Error("Failed to fetch budgets");
                }
            } catch (error) {
                console.log(error);
            }
        };
      
        fetchBudgets();
    }, [selectedUser])

    useEffect(() => {

        const fetchExpenseTypes = async () => {
            try {
                const expenseTypesResponse = await fetch(`api/ExpenseType`);
                if (expenseTypesResponse.status === 200) {
                    const expenseTypesData = await expenseTypesResponse.json();
                    setExpenseTypes(expenseTypesData);
                } else {
                    throw new Error("Failed to fetch expense types");
                }
            } catch (error) {
                console.log(error);
            }
        };
      
        fetchExpenseTypes();
      }, []);

    useEffect(() => {

        const fetchData = async () => {
            try {
                const expensesResponse = await fetch(`api/Expense/user/${selectedUser.userId}`);
                if (expensesResponse.status === 200) {
                    const expensesData = await expensesResponse.json();

                    console.log("expensesData: ", expensesData);
                    
                    const newExpenses = await Promise.all(
                        expensesData.map(async (expense) => {
                            const expenseTypeResponse = await fetch(`api/ExpenseType/${expense.expenseTypeId}`);
                            if (expenseTypeResponse.status === 200) {
                                const expenseTypeData = await expenseTypeResponse.json();

                                console.log("expenseTypeData: ", expenseTypeData);
                                
                                const responseDate = new Date(expense.date);

                                return {
                                    key: expense.id,
                                    name: expense.name,
                                    value: expense.value,
                                    date: `${responseDate.getFullYear()}-${responseDate.getMonth() + 1}-${responseDate.getDate()}`,
                                    userId: expense.userId,
                                    expenseTypeId: expense.expenseTypeId,
                                    expenseType: expenseTypeData.name,
                                };
                            } else {
                                throw new Error("Failed to fetch expense type");
                            }
                        })
                    );
                    setExpenses(newExpenses);
                } else {
                    throw new Error("Failed to fetch user expenses");
                }
            } 
            catch (error) {
                console.log(error);
            }
        };
    
        fetchData();
      }, [selectedUser]);

    const handleAddExpense = () => {
        form.resetFields();
        setSelectedExpense(null);
        setSelectedExpenseType(null);
        setModalVisible(true);
    };

    const handleEditExpense = (expense) => {
        form.resetFields();
        setSelectedExpense(expense);
        setSelectedExpenseType(expense.expenseTypeId);
        setModalVisible(true);
    };

    const handleDeleteExpense = (expenseId) => {
        // Вызов API для удаления расхода
        fetch(`api/Expense/${expenseId}`, {
            method: "DELETE",
        })
            .then((response) => {
                if (response.status === 204) {
                    // Удаление расхода из списка после успешного удаления
                    setExpenses((prevExpenses) =>
                        prevExpenses.filter((expense) => expense.key !== expenseId)
                    );
                } else {
                    throw new Error("Failed to delete expense");
                }
            })
            .catch((error) => {
                console.log(error);
            });
    };

    const handleModalCancel = () => {
        form.resetFields();
        setSelectedExpense(null);
        setSelectedExpenseType(null); 
        setModalVisible(false);
    };

    const handleModalOk = () => {
        form.validateFields().then((values) => {
            const newExpense = {
                key: selectedExpense ? selectedExpense.key : Date.now(),
                name: values.name,
                value: parseFloat(values.value),
                date: values.date.format("YYYY-MM-DD"),
                expenseTypeId: values.expenseTypeId,
                userId: userId
            };

            if (selectedExpense) {
                console.log("Selected Expense: ", selectedExpense);
                console.log("New Expense: ", newExpense);

                // вызов API для обновления расхода
                fetch(`api/Expense/${selectedExpense.key}`, {
                    method: "PUT",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify({
                        id: newExpense.key,
                        name: newExpense.name,
                        value: newExpense.value,
                        date: newExpense.date,
                        userId: newExpense.userId,
                        expenseTypeId: newExpense.expenseTypeId,
                        expenseType: ""
                    }),
                })
                    .then((response) => {
                        if (response.status === 204) {

                            // Обновление списка расходов после успешного обновления
                            fetch(`api/ExpenseType/${newExpense.expenseTypeId}`)
                                .then((expenseTypeResponse) => {
                                    if (expenseTypeResponse.status === 200) {
                                        return expenseTypeResponse.json();
                                    } else {
                                        throw new Error("Failed to fetch expense type");
                                    }
                                })
                                .then((expenseTypeData) => {
                                    newExpense.expenseType = expenseTypeData.name;
                                    setExpenses((prevExpenses) =>
                                        prevExpenses.map((expense) =>
                                            expense.key === selectedExpense.key ? newExpense : expense
                                        )
                                    );
                                })
                                .catch((error) => {
                                    console.log(error);
                                });
                        } else {
                            throw new Error("Failed to update expense");
                        }
                    })
                    .catch((error) => {
                        console.log(error);
                    });
            } else {

                // Вызов API для создания расхода
                fetch("api/Expense", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify({
                        name: newExpense.name,
                        value: newExpense.value,
                        date: newExpense.date,
                        userId: newExpense.userId,
                        expenseTypeId: newExpense.expenseTypeId,
                        expenseType: ""
                    }),
                })
                    .then((response) => {
                        if (response.ok) {
                            return response.json();
                        } else {
                            throw new Error("Failed to create expense");
                        }
                    })
                    .then((createdExpense) => {
                        console.log("New Expense: ", newExpense);

                        // Обновление списка расходов после успешного создания
                        fetch(`api/ExpenseType/${newExpense.expenseTypeId}`)
                            .then((expenseTypeResponse) => {
                                if (expenseTypeResponse.status === 200) {
                                    return expenseTypeResponse.json();
                                } else {
                                    throw new Error("Failed to fetch expense type");
                                }
                            })
                            .then((expenseTypeData) => {
                                newExpense.expenseType = expenseTypeData.name;

                                fetch(`api/Expense/user/${userId}`)
                                    .then((allExpensesResponse) => {
                                        if (allExpensesResponse.status === 200)
                                        {
                                            return allExpensesResponse.json();
                                        }
                                        else
                                        {
                                            throw new Error("Falied to fetch allExpenses after creating expense");
                                        }
                                    })
                                    .then((allExpensesData) => {
                                        newExpense.key = allExpensesData[allExpensesData.length - 1].id;
                                        console.log(newExpense);
                                    })
                                    .catch((error) => {
                                        console.log(error);
                                    });
                                setExpenses((prevExpenses) => [...prevExpenses, newExpense]);
                            })
                            .catch((error) => {
                                console.log(error);
                            });
                    })
                    .catch((error) => {
                        console.log(error);
                    });
            }

            form.resetFields();
            setSelectedExpenseType(null); 
            setModalVisible(false);
        });
    };

    useEffect(() => {
        if ( isFilteredExpensesDataEditing === true &&
            filteredExpensesData !== null &&
            filteredExpensesData[filteredExpensesData.length - 1]?.budget?.startDate !== undefined
        )
        {
            setIsFilteredExpensesDataSets(true);
        }
    }, [isFilteredExpensesDataEditing, filteredExpensesData])

    useEffect(() => {
        if(isFilteredExpensesDataSets === true)
        {
            setReportVisible(true);
            setIsFilteredExpensesDataSets(false);
        }
    }, [isFilteredExpensesDataSets])

    const handleReportOpen = async () => {
        setIsFilteredExpensesDataEditing(false);
        setIsFilteredExpensesDataSets(false);

        await fetch(`api/User/${selectedUser.userId}`)
            .then((response) => {
                if (response.status === 200)
                {
                    return response.json();                   
                }
                else
                {
                    throw new Error("Failed to fetch user");
                }
            })
            .then((userData) => {
                console.log(userData);

                const user = userData;

                fetch(`api/Report/difference`, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify({
                        id: user.id,
                        userName: user.userName,
                        password: "",
                        role: user.role
                    }),
                })
                    .then((response) => {
                        if (response.status === 200)
                        {
                            return response.json();                   
                        }
                        else
                        {
                            throw new Error("Failed to fetch difference");
                        }
                    })
                    .then((data) => {
                        console.log("Data: ", data);

                        // Создание нового массива со значениями, типами расходов и бюджетами
                        const expensesData = data.map((arr, index) => {
                            const budget = budgets[index]; 
                            return arr.map((value, i) => {
                                const expenseType = expenseTypes[i]; 
                                return { value, expenseType, budget };
                            });
                        });
                        console.log(expensesData);

                        // отфильтрованные данные
                        const filteredData = expensesData.flat().filter((item) => item.value > 0);
                        if(filteredData.length === 0 || filteredData === null)
                        {
                            setReportVisible(true);
                        }
                        else
                        {
                            setFilteredExpensesData(filteredData);
                            setIsFilteredExpensesDataEditing(true);
                            console.log(filteredExpensesData);
                        }
                    })
                    .catch((error) => 
                    {
                        console.log(error);
                    })     
            })
            .catch((error) => 
            {
                console.log(error);
            })        

    }

    const handleReportCancel = () => {
        setReportVisible(false);
    }

    const columns = [
        { title: "Название", dataIndex: "name" },
        { title: "Сумма (в рублях)", dataIndex: "value" },
        { title: "Дата", dataIndex: "date" },
        { title: "Тип расхода", dataIndex: "expenseType"},
        {
            render: (_, expense) => (
                <>
                    <Button onClick={() => handleEditExpense(expense)}>Редактировать</Button>
                    <Button onClick={() => handleDeleteExpense(expense.key)}>Удалить</Button>
                </>
            ),
        },
    ];

    const selectedDate = selectedExpense ? moment(selectedExpense.date) : null;

    return (
        <>
            <Button onClick={handleAddExpense} style={{ marginBottom: 16 }} className="addButton">
                Добавить расход
            </Button>

            {userRole === "admin" && selectedUser !== null && (
                <Select
                    value={selectedUser.userId}
                    onChange={(userId) => {
                        const newSelectedUser = users.find(user => user.userId === userId);
                        setSelectedUser(newSelectedUser);
                    }}
                    style={{ marginLeft: 10 ,marginBottom: 16 }}
                    className="userSelect"
                    dropdownMatchSelectWidth={false}
                >
                    {users.map((user) => (
                        <Select.Option key={user.userId} value={user.userId}>
                            {user.userName}
                        </Select.Option>
                    ))}
                </Select>
            )}

            <Button onClick={handleReportOpen} style={{ marginBottom: 16 }} className="getDifferences">
                Превышения бюджетов
            </Button>

            <Table dataSource={expenses} columns={columns} />
            <Modal
                destroyOnClose={true}
                title={selectedExpense ? "Редактирование расхода" : "Добавление расхода"}
                open={modalVisible}
                onOk={handleModalOk}
                onCancel={handleModalCancel}
            >
                <Form form={form} layout="vertical">
                    <Form.Item
                        name="name"
                        label="Название"
                        initialValue={selectedExpense ? selectedExpense.name : ""}
                        rules={[{ required: true, message: "Введите название расхода" }]}
                    >
                        <Input />
                    </Form.Item>

                    <Form.Item
                        name="value"
                        label="Сумма (в рублях)"
                        initialValue={selectedExpense ? selectedExpense.value : ""}
                        rules={[{ required: true, message: "Введите значение расхода" }]}
                    >
                        <Input type="number" step="1" />
                    </Form.Item>

                    <Form.Item
                        name="date"
                        label="Дата"
                        initialValue={selectedDate}
                        rules={[{ required: true, message: "Введите дату" }]}
                    >
                        <DatePicker format="YYYY-MM-DD" showTime={false}/>
                    </Form.Item>

                    <Form.Item
                        name="expenseTypeId"
                        label="Тип дохода"
                        initialValue={selectedExpenseType}
                        rules={[{ required: true, message: "Выберите тип расхода" }]}
                    >
                        <Select
                            showSearch
                            placeholder="Выберите тип расхода"
                            optionFilterProp="children"
                            onChange={setSelectedExpenseType}
                            filterOption={(input, option) =>
                                option.children.toLowerCase().indexOf(input.toLowerCase()) >= 0
                            }
                        >
                            {expenseTypes.map((expenseType) => (
                                <Select.Option key={expenseType.id} value={expenseType.id}>
                                    {expenseType.name}
                                </Select.Option>
                            ))}
                        </Select>
                    </Form.Item>
                </Form>
            </Modal>

            <Modal
                destroyOnClose={true}
                title="Отчёт"
                open={reportVisible}
                onCancel={handleReportCancel}
                footer={null}              
            >
                {filteredExpensesData && filteredExpensesData.length > 0 ? (
                    filteredExpensesData.map((item, index) => (
                    <div key={index}>
                        <p>Категория {item.expenseType.name} превысила бюджет с {item.budget.startDate} на {item.value} рубля</p>
                    </div>
                    ))
                ) : (
                    <p>Нет превышений бюджетов</p>
                )}
            </Modal>

        </>
    );
};

export default Expenses;