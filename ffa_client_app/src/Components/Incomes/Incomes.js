import React, { useState, useEffect } from "react";
import { Table, Button, Modal, Form, Input, DatePicker, Select } from "antd";
import moment from "moment/moment";

import "./Incomes.css"

const Incomes = ({ userId }) => {
    const [incomes, setIncomes] = useState([]);
    const [selectedIncome, setSelectedIncome] = useState(null);
    const [modalVisible, setModalVisible] = useState(false);
    const [incomeTypes, setIncomeTypes] = useState([]);
    const [selectedIncomeType, setSelectedIncomeType] = useState(null);
    const [form] = Form.useForm();

    useEffect(() => {

        const fetchIncomeTypes = async () => {
            try {
                const incomeTypesResponse = await fetch(`api/IncomeType`);
                if (incomeTypesResponse.status === 200) {
                    const incomeTypesData = await incomeTypesResponse.json();
                    setIncomeTypes(incomeTypesData);
                } else {
                    throw new Error("Failed to fetch income types");
                }
            } catch (error) {
                console.log(error);
            }
        };
      
        fetchIncomeTypes();
      }, []);

    useEffect(() => {

        const fetchData = async () => {
            try {
                const incomesResponse = await fetch(`api/Income/user/${userId}`);
                if (incomesResponse.status === 200) {
                    const incomesData = await incomesResponse.json();

                    console.log("incomesData: ", incomesData);
                    
                    const newIncomes = await Promise.all(
                        incomesData.map(async (income) => {
                            const incomeTypeResponse = await fetch(`api/IncomeType/${income.incomeTypeId}`);
                            if (incomeTypeResponse.status === 200) {
                                const incomeTypeData = await incomeTypeResponse.json();

                                console.log("incomeTypeData: ", incomeTypeData);
                                
                                return {
                                    key: income.id,
                                    name: income.name,
                                    value: income.value,
                                    date: income.date,
                                    userId: income.userId,
                                    incomeTypeId: income.incomeTypeId,
                                    incomeType: incomeTypeData.name,
                                };
                            } else {
                                throw new Error("Failed to fetch income type");
                            }
                        })
                    );
                    setIncomes(newIncomes);
                } else {
                    throw new Error("Failed to fetch user incomes");
                }
            } 
            catch (error) {
                console.log(error);
            }
        };
    
        fetchData();
    }, []);

    const handleAddIncome = () => {
        form.resetFields();
        setSelectedIncome(null);
        setSelectedIncomeType(null);
        setModalVisible(true);
    };

    const handleEditIncome = (income) => {
        form.resetFields();
        setSelectedIncome(income);
        setSelectedIncomeType(income.incomeTypeId);
        setModalVisible(true);
    };

    const handleDeleteIncome = (incomeId) => {
        // Вызов API для удаления дохода
        fetch(`api/Income/${incomeId}`, {
            method: "DELETE",
        })
            .then((response) => {
                if (response.status === 204) {
                    // Удаление дохода из списка после успешного удаления
                    setIncomes((prevIncomes) =>
                        prevIncomes.filter((income) => income.key !== incomeId)
                    );
                } else {
                    throw new Error("Failed to delete income");
                }
            })
            .catch((error) => {
                console.log(error);
            });
    };

    const handleModalCancel = () => {
        form.resetFields();
        setSelectedIncome(null);
        setSelectedIncomeType(null); 
        setModalVisible(false);
    };

    const handleModalOk = () => {
        form.validateFields().then((values) => {
            const newIncome = {
                key: selectedIncome ? selectedIncome.key : Date.now(),
                name: values.name,
                value: parseFloat(values.value),
                date: values.date.format("YYYY-MM-DD"),
                incomeTypeId: values.incomeTypeId,
                userId: userId
            };

            if (selectedIncome) {
                console.log("Selected Income: ", selectedIncome);
                console.log("New Income: ", newIncome);

                // Вызов API для обновления дохода
                fetch(`api/Income/${selectedIncome.key}`, {
                    method: "PUT",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify({
                        id: newIncome.key,
                        name: newIncome.name,
                        value: newIncome.value,
                        date: newIncome.date,
                        userId: newIncome.userId,
                        incomeTypeId: newIncome.incomeTypeId,
                        IncomeType: ""
                    }),
                })
                    .then((response) => {
                        if (response.status === 204) {

                            // Обновление списка доходов после успешного обновления
                            fetch(`api/IncomeType/${newIncome.incomeTypeId}`)
                                .then((incomeTypeResponse) => {
                                    if (incomeTypeResponse.status === 200) {
                                        return incomeTypeResponse.json();
                                    } else {
                                        throw new Error("Failed to fetch income type");
                                    }
                                })
                                .then((incomeTypeData) => {
                                    newIncome.incomeType = incomeTypeData.name;
                                    setIncomes((prevIncomes) =>
                                        prevIncomes.map((income) =>
                                            income.key === selectedIncome.key ? newIncome : income
                                        )
                                    );
                                })
                                .catch((error) => {
                                    console.log(error);
                                });
                        } else {
                            throw new Error("Failed to update income");
                        }
                    })
                    .catch((error) => {
                        console.log(error);
                    });
            } else {

                // вызов API для создания дохода
                fetch("api/Income", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify({
                        name: newIncome.name,
                        value: newIncome.value,
                        date: newIncome.date,
                        userId: newIncome.userId,
                        incomeTypeId: newIncome.incomeTypeId,
                        incomeType: ""
                    }),
                })
                    .then((response) => {
                        if (response.ok) {
                            return response.json();
                        } else {
                            throw new Error("Failed to create income");
                        }
                    })
                    .then((createdIncome) => {
                        console.log("New Income: ", newIncome);

                        // Обновление списка расходов после успешного создания
                        fetch(`api/IncomeType/${newIncome.incomeTypeId}`)
                            .then((incomeTypeResponse) => {
                                if (incomeTypeResponse.status === 200) {
                                    return incomeTypeResponse.json();
                                } else {
                                    throw new Error("Failed to fetch income type");
                                }
                            })
                            .then((incomeTypeData) => {
                                newIncome.incomeType = incomeTypeData.name;

                                fetch(`api/Income/user/${userId}`)
                                    .then((allIncomesResponse) => {
                                        if (allIncomesResponse.status === 200)
                                        {
                                            return allIncomesResponse.json();
                                        }
                                        else
                                        {
                                            throw new Error("Falied to fetch allIncomes after creating income");
                                        }
                                    })
                                    .then((allIncomesData) => {
                                        newIncome.key = allIncomesData[allIncomesData.length - 1].id;
                                        console.log(newIncome);
                                    })
                                    .catch((error) => {
                                        console.log(error);
                                    });
                                    setIncomes((prevIncomes) => [...prevIncomes, newIncome]);
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
            setSelectedIncomeType(null); 
            setModalVisible(false);
        });
    };


    const columns = [
        { title: "Название", dataIndex: "name" },
        { title: "Сумма (в рублях)", dataIndex: "value" },
        { title: "Дата", dataIndex: "date" },
        { title: "Тип дохода", dataIndex: "incomeType"},
        {
            render: (_, income) => (
                <>
                    <Button onClick={() => handleEditIncome(income)}>Редактировать</Button>
                    <Button onClick={() => handleDeleteIncome(income.key)}>Удалить</Button>
                </>
            ),
        },
    ];

    const selectedDate = selectedIncome ? moment(selectedIncome.date) : null;

    return (
        <>
            <Button onClick={handleAddIncome} style={{ marginBottom: 16 }} className="addButton">
                Добавить доход
            </Button>

            <Table dataSource={incomes} columns={columns} />
            <Modal
                destroyOnClose={true}
                title={selectedIncome ? "Редактирование дохода" : "Добавление дохода"}
                open={modalVisible}
                onOk={handleModalOk}
                onCancel={handleModalCancel}
            >
                <Form form={form} layout="vertical">
                    <Form.Item
                        name="name"
                        label="Название"
                        initialValue={selectedIncome ? selectedIncome.name : ""}
                        rules={[{ required: true, message: "Введите название дохода" }]}
                    >
                        <Input />
                    </Form.Item>

                    <Form.Item
                        name="value"
                        label="Сумма (в рублях)"
                        initialValue={selectedIncome ? selectedIncome.value : ""}
                        rules={[{ required: true, message: "Введите значение дохода" }]}
                    >
                        <Input type="number" step="0.01" />
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
                        name="incomeTypeId"
                        label="Тип дохода"
                        initialValue={selectedIncomeType}
                        rules={[{ required: true, message: "Выберите тип дохода" }]}
                    >
                        <Select
                            showSearch
                            placeholder="Выберите тип дохода"
                            optionFilterProp="children"
                            onChange={setSelectedIncomeType}
                            filterOption={(input, option) =>
                                option.children.toLowerCase().indexOf(input.toLowerCase()) >= 0
                            }
                        >
                            {incomeTypes.map((incomeType) => (
                                <Select.Option key={incomeType.id} value={incomeType.id}>
                                    {incomeType.name}
                                </Select.Option>
                            ))}
                        </Select>
                    </Form.Item>
                </Form>
            </Modal>
        </>
    );
};

export default Incomes;