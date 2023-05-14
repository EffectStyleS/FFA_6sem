import React, { useState, useEffect } from "react";
import { Table, Button, Modal, Form, Input, DatePicker, Select } from "antd";
import moment from "moment/moment";

import "./Expenses.css"

const Expenses = ({ userId }) => {
    const [expenses, setExpenses] = useState([]);
    const [selectedExpense, setSelectedExpense] = useState(null);
    const [modalVisible, setModalVisible] = useState(false);
    const [expenseTypes, setExpenseTypes] = useState([]);
    const [selectedExpenseType, setSelectedExpenseType] = useState(null);
    const [form] = Form.useForm();


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
                const expensesResponse = await fetch(`api/Expense/user/${userId}`);
                if (expensesResponse.status === 200) {
                    const expensesData = await expensesResponse.json();

                    console.log("expensesData: ", expensesData);
                    
                    const newExpenses = await Promise.all(
                        expensesData.map(async (expense) => {
                            const expenseTypeResponse = await fetch(`api/ExpenseType/${expense.expenseTypeId}`);
                            if (expenseTypeResponse.status === 200) {
                                const expenseTypeData = await expenseTypeResponse.json();

                                console.log("expenseTypeData: ", expenseTypeData);
                                
                                return {
                                    key: expense.id,
                                    name: expense.name,
                                    value: expense.value,
                                    date: expense.date,
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
      }, []);

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
                        <DatePicker format="YYYY-MM-DD" />
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
        </>
    );
};

export default Expenses;