import React, { useState, useEffect } from "react";
import { Table, Button, Modal, Form, Input, DatePicker, Select, Row, Col } from "antd";
import moment from "moment/moment";

import "./Budgets.css"

const Budgets = ({ userId }) => {
    const [budgets, setBudgets] = useState([]);
    const [selectedBudget, setSelectedBudget] = useState(null);

    const [modalVisible, setModalVisible] = useState(false);

    const [timePeriods, setTimPeriods] = useState([]);
    const [selectedTimePeriod, setSelectedTimePeriod] = useState(null);

    const [incomeTypes, setIncomeTypes] = useState([]);
    const [expenseTypes, setExpenseTypes] = useState([]);

    const [listOfPlannedExpenses, setListOfPlannedExpenses] = useState([]);
    const [isPlannedExpensesEditing, setIsPlannedExpensesEditing] = useState(false);
    const [isPlannedExpensesSets, setIsPlannedExpensesSets] = useState(false);
    const [isPlannedExpensesDeleting, setIsPlannedExpensesDeleting] = useState(false);

    const [listOfPlannedIncomes, setListOfPlannedIncomes] = useState([]);
    const [isPlannedIncomesEditing, setIsPlannedIncomesEditable] = useState(false);
    const [isPlannedIncomesSets, setIsPlannedIncomesSets] = useState(false);
    const [isPlannedIncomesDeleting, setIsPlannedIncomesDeleting] = useState(false);

    const [canDelete, setCanDelete] = useState(false);

    const [form] = Form.useForm();

    useEffect(() => {

        const fetchIncomeTypes = async () => {
            try {
                const incomeTypesResponse = await fetch(`api/IncomeType`);
                if (incomeTypesResponse.status === 200) {
                    const incomeTypesData = await incomeTypesResponse.json();
                    setIncomeTypes(incomeTypesData);

                    // Заполняем listOfPlannedIncomes
                    const plannedIncomes = incomeTypesData.map((incomeType) => ({
                        incomeTypeId: incomeType.id,
                        sum: 0,
                        budgetId: 0
                    }));
                    setListOfPlannedIncomes(plannedIncomes);
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

        const fetchExpenseTypes = async () => {
            try {
                const expenseTypesResponse = await fetch(`api/ExpenseType`);
                if (expenseTypesResponse.status === 200) {
                    const expenseTypesData = await expenseTypesResponse.json();
                    setExpenseTypes(expenseTypesData);

                    // Заполняем listOfPlannedExpenses
                    const plannedExpenses = expenseTypesData.map((expenseType) => ({
                        expenseTypeId: expenseType.id,
                        sum: 0,
                        budgetId: 0
                    }));
                    setListOfPlannedExpenses(plannedExpenses);
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

        const fetchTimePeriods = async () => {
            try {
                const timePeriodsResponse = await fetch(`api/TimePeriod`);
                if (timePeriodsResponse.status === 200) {
                    const timePeriodsData = await timePeriodsResponse.json();
                    setTimPeriods(timePeriodsData);
                } else {
                    throw new Error("Failed to fetch timePeriods")
                }
            } catch (error) {
                console.log(error);
            }
        };

        fetchTimePeriods();
    }, []); 

    useEffect(() => {

        const fetchData = async () => {
            try {
                const budgetsResponse = await fetch (`api/Budget/user/${userId}`);
                if (budgetsResponse.status === 200) {
                    const budgetsData = await budgetsResponse.json();

                    console.log("budgetsData: ", budgetsData);

                    const newBudgets = await Promise.all(
                        budgetsData.map(async (budget) => {
                            const timePeriodResponse = await fetch(`api/TimePeriod/${budget.timePeriodId}`);
                            if (timePeriodResponse.status === 200) {
                                const timePeriodData = await timePeriodResponse.json();

                                console.log("timePeriodData: ", timePeriodData);

                                const responseDate = new Date(budget.startDate);

                                return {
                                    key: budget.id,
                                    startDate: `${responseDate.getFullYear()}-${responseDate.getMonth() + 1}-${responseDate.getDate()}`,
                                    timePeriodId: budget.timePeriodId,
                                    userId: budget.userId,
                                    timePeriod: timePeriodData.name
                                };
                            } else {
                                throw new Error("Failed to fetch time period");
                            }
                        })
                    );
                    setBudgets(newBudgets);
                } else {
                    throw new Error("Falied to fetch user budgets");
                }
            }
            catch(error) {
                console.log(error);
            } 
        };

        fetchData();
    }, []);


    useEffect(() => {
        if ( (isPlannedExpensesEditing === true || isPlannedExpensesDeleting === true) &&
            listOfPlannedExpenses[listOfPlannedExpenses.length - 1].budgetId !== 0 &&
            typeof listOfPlannedExpenses[listOfPlannedExpenses.length - 1].id !== "undefined" &&
            listOfPlannedExpenses[listOfPlannedExpenses.length - 1].id !== 0
        )
            {
                setIsPlannedExpensesSets(true);
            }
    }, [listOfPlannedExpenses, isPlannedExpensesEditing, isPlannedExpensesDeleting])

    useEffect(() => {
        if ( (isPlannedIncomesEditing === true || isPlannedIncomesDeleting === true) &&
            listOfPlannedIncomes[listOfPlannedIncomes.length - 1].budgetId !== 0 &&
            typeof listOfPlannedIncomes[listOfPlannedIncomes.length - 1].id !== "undefined" &&
            listOfPlannedIncomes[listOfPlannedIncomes.length - 1].id !== 0
        )
            {
                setIsPlannedIncomesSets(true);
            }
    }, [listOfPlannedIncomes, isPlannedIncomesEditing, isPlannedIncomesDeleting])

    useEffect(() => {
        if (isPlannedIncomesEditing === true &&
            isPlannedExpensesEditing === true &&
            isPlannedExpensesSets === true &&
            isPlannedIncomesSets === true 
        )
        {
            setModalVisible(true);
            setIsPlannedExpensesSets(false);
            setIsPlannedIncomesSets(false);
        }
    }, [isPlannedExpensesSets, isPlannedIncomesSets, isPlannedIncomesEditing, isPlannedExpensesEditing])

    useEffect(() => {
        if (isPlannedIncomesDeleting === true &&
            isPlannedExpensesDeleting === true &&
            isPlannedExpensesSets === true &&
            isPlannedIncomesSets === true 
        )
        {
            setCanDelete(true);
            setIsPlannedExpensesSets(false);
            setIsPlannedIncomesSets(false);
        }
    }, [isPlannedExpensesSets, isPlannedIncomesSets, isPlannedIncomesDeleting, isPlannedExpensesDeleting])

    const handleAddBudget = () => {
        form.resetFields();
        setIsPlannedExpensesEditing(false);
        setIsPlannedIncomesEditable(false);
        setSelectedBudget(null);
        setSelectedTimePeriod(null);
        
        // Заполняем listOfPlannedExpenses
        const plannedExpenses = expenseTypes.map((expenseType) => ({
            expenseTypeId: expenseType.id,
            sum: 0,
            budgetId: 0
        }));
        setListOfPlannedExpenses(plannedExpenses);
        console.log("List of PlannedExpenses: ", listOfPlannedExpenses)

        // Заполняем listOfPlannedIncomes
        const plannedIncomes = incomeTypes.map((incomeType) => ({
            incomeTypeId: incomeType.id,
            sum: 0,
            budgetId: 0
        }));
        setListOfPlannedIncomes(plannedIncomes);
        console.log("List of PlannedIncomes: ", listOfPlannedIncomes)

        setModalVisible(true);
    };

    const handleEditBudget = (budget) => {
        form.resetFields();
        setSelectedBudget(budget);
        setSelectedTimePeriod(budget.timePeriodId);

        // Заполняем listOfPlannedExpenses
        const plannedExpenses = expenseTypes.map((expenseType) => ({
            id: 0,
            expenseTypeId: expenseType.id,
            sum: 0,
            budgetId: budget.key
        }));

        // Обновление id запланированных доходов
        fetch(`api/PlannedExpenses/budget/${budget.key}`)
            .then((plannedExpensesResponse) => {
                if (plannedExpensesResponse.status === 200) {
                    return plannedExpensesResponse.json()
                } else {
                    throw new Error("Failed to fetch planned expenses");
                }
            })
            .then((plannedExpensesData) => {
                console.log("Data: ", plannedExpensesData);
                
                for (let j = 0; j < plannedExpensesData.length; j++) {
                    plannedExpenses[j].id = plannedExpensesData[j].id; 
                    plannedExpenses[j].sum = plannedExpensesData[j].sum;
                    plannedExpenses[j].expenseTypeId = plannedExpensesData[j].expenseTypeId; 
                }
                console.log("New List Of PlannedExpenses: ", plannedExpenses);

                setListOfPlannedExpenses(plannedExpenses);
                setIsPlannedExpensesEditing(true);
            })

        console.log("List of PlannedExpenses: ", listOfPlannedExpenses)

        
        // Заполняем listOfPlannedIncomes
        const plannedIncomes = incomeTypes.map((incomeType) => ({
            id: 0,
            incomeTypeId: incomeType.id,
            sum: 0,
            budgetId: budget.key
        }));
        console.log("List of PlannedIncomes: ", listOfPlannedIncomes)

        // Обновление id запланированных расходов
        fetch(`api/PlannedIncomes/budget/${budget.key}`)
            .then((plannedIcnomesResponse) => {
                if (plannedIcnomesResponse.status === 200) {
                    return plannedIcnomesResponse.json()
                } else {
                    throw new Error("Failed to fetch planned incomes");
                }
            })
            .then((plannedIncomesData) => {
                console.log("Data: ", plannedIncomesData);
     
                for (let j = 0; j < plannedIncomesData.length; j++) {
                    plannedIncomes[j].id = plannedIncomesData[j].id; 
                    plannedIncomes[j].sum = plannedIncomesData[j].sum; 
                    plannedIncomes[j].incomeTypeId = plannedIncomesData[j].incomeTypeId;
                }
                console.log("New List Of PlannedIncomes: ", plannedIncomes);

                setListOfPlannedIncomes(plannedIncomes);
                setIsPlannedIncomesEditable(true);
            })   

        //setModalVisible(true);

    };

    const handleDeleteBudget = (budgetId) => {
        setIsPlannedExpensesEditing(false);
        setIsPlannedIncomesEditable(false);
        setIsPlannedExpensesDeleting(false);
        setIsPlannedIncomesDeleting(false)
      
        // Заполняем listOfPlannedExpenses
        const plannedExpenses = expenseTypes.map((expenseType) => ({
            id: 0,
            expenseTypeId: expenseType.id,
            sum: 0,
            budgetId: budgetId
        }));

        setListOfPlannedExpenses(plannedExpenses);

        // Заполняем listOfPlannedIncomes
        const plannedIncomes = incomeTypes.map((incomeType) => ({
            id: 0,
            incomeTypeId: incomeType.id,
            sum: 0,
            budgetId: budgetId
        }));
        console.log("List of PlannedIncomes: ", listOfPlannedIncomes)

        setListOfPlannedIncomes(plannedIncomes);

        // Обновление id запланированных доходов
        const deletePlannedExpensesPromise = fetch(`api/PlannedExpenses/budget/${budgetId}`)
            .then((plannedExpensesResponse) => {
                if (plannedExpensesResponse.status === 200) {
                    return plannedExpensesResponse.json();
                } else {
                    throw new Error("Failed to fetch planned expenses");
                }
            })
            .then((plannedExpensesData) => {
                console.log("Data: ", plannedExpensesData);
                
                for (let j = 0; j < plannedExpensesData.length; j++) {
                    plannedExpenses[j].id = plannedExpensesData[j].id; 
                    plannedExpenses[j].sum = plannedExpensesData[j].sum;
                    plannedExpenses[j].expenseTypeId = plannedExpensesData[j].expenseTypeId; 
                }
                console.log("New List Of PlannedExpenses: ", plannedExpenses);

                setListOfPlannedExpenses(plannedExpenses);
                setIsPlannedExpensesDeleting(true);
            })

        console.log("List of PlannedExpenses: ", listOfPlannedExpenses)

        // Обновление id запланированных расходов
        const deletePlannedIncomesPromise = fetch(`api/PlannedIncomes/budget/${budgetId}`)
            .then((plannedIncomesResponse) => {
                if (plannedIncomesResponse.status === 200) {
                    return plannedIncomesResponse.json();
                } else {
                    throw new Error("Failed to fetch planned incomes");
                }
            })
            .then((plannedIncomesData) => {
                console.log("Data: ", plannedIncomesData);
        
                for (let j = 0; j < plannedIncomesData.length; j++) {
                    plannedIncomes[j].id = plannedIncomesData[j].id; 
                    plannedIncomes[j].sum = plannedIncomesData[j].sum; 
                    plannedIncomes[j].incomeTypeId = plannedIncomesData[j].incomeTypeId;
                }
                console.log("New List Of PlannedIncomes: ", plannedIncomes);

                setListOfPlannedIncomes(plannedIncomes);
                setIsPlannedIncomesDeleting(true);
            })
            
        if (canDelete === true) {
            Promise.all([deletePlannedExpensesPromise, deletePlannedIncomesPromise])
                .then(() => {
                // Сначала вызвать API для удаления запланированных доходов и расходов
                    Promise.all([
                        // Вызов API для удаления запланированных расходов
                        ...listOfPlannedExpenses.map((plannedExpense) =>
                        fetch(`api/PlannedExpenses/${plannedExpense.id}`, {
                            method: "DELETE",
                        })
                        ),
                        // Вызов API для удаления запланированных доходов
                        ...listOfPlannedIncomes.map((plannedIncome) =>
                        fetch(`api/PlannedIncomes/${plannedIncome.id}`, {
                            method: "DELETE",
                        })
                        ),
                    ])
                        .then((responses) => {
                        const successfulResponses = responses.filter(
                            (response) => response.status === 204
                        );
                        if (successfulResponses.length === responses.length) {
                            // Все удаления запланированных доходов и расходов прошли успешно
                
                            // Вызов API для удаления бюджета
                            fetch(`api/Budget/${budgetId}`, {
                            method: "DELETE",
                            })
                            .then((response) => {
                                if (response.status === 204) {
                                // Удаление бюджета из списка после успешного удаления
                                setBudgets((prevBudgets) =>
                                    prevBudgets.filter((budget) => budget.key !== budgetId)
                                );
                                } else {
                                throw new Error("Failed to delete budget");
                                }
                            })
                            .catch((error) => {
                                console.log(error);
                            });
                        } else {
                            throw new Error("Failed to delete planned expenses and incomes");
                        }
                        })
                        .catch((error) => {
                        console.log(error);
                        });
                    })
                .catch((error) => {
                    console.log(error);
                });
            
            setCanDelete(false);
            }
      };

    const handleModelCancel = () => {
        form.resetFields();
        setSelectedBudget(null);
        setSelectedTimePeriod(null);
        setModalVisible(false);
        setIsPlannedExpensesSets(false);
        setIsPlannedIncomesSets(false);
    };

    const handleModalOk = () => {
        form.validateFields().then((values) => {
            const newBudget = {
                key: selectedBudget ? selectedBudget.key : Date.now(),
                startDate: values.startDate.format("YYYY-MM-DD"),
                timePeriodId: values.timePeriodId,
                userId: userId
            };

            const newListOfPlannedExpenses = expenseTypes.map((expenseType) => {
                const matchingExpense = listOfPlannedExpenses.find((plannedExpense) => plannedExpense.expenseTypeId === expenseType.id);
                const id = matchingExpense ? matchingExpense.id : 0;
                const expenseTypeId = matchingExpense ? matchingExpense.expenseTypeId : expenseType.id;
                const sum = matchingExpense ? values[`expense_${matchingExpense.expenseTypeId}`] : 0;
              
                return {
                    id,
                    expenseTypeId,
                    sum,
                    budgetId: matchingExpense ? matchingExpense.budgetId : newBudget.key
                };
            });             
            console.log("newListOfPlannedExpenses after OK: ", newListOfPlannedExpenses);

            const newListOfPlannedIncomes = incomeTypes.map((incomeType) => {
                const matchingIncome = listOfPlannedIncomes.find((plannedIncome) => plannedIncome.incomeTypeId === incomeType.id);
                const id = matchingIncome ? matchingIncome.id : 0;
                const incomeTypeId = matchingIncome ? matchingIncome.incomeTypeId : incomeType.id;
                const sum = matchingIncome ? values[`income_${matchingIncome.incomeTypeId}`] : 0;
              
                return {
                    id,
                    incomeTypeId,
                    sum,
                    budgetId: matchingIncome ? matchingIncome.budgetId : newBudget.key
                };
            });             
            console.log("newListOfPlannedIncomes after OK: ", newListOfPlannedIncomes);

            if (selectedBudget) {
                console.log("Selected budget: ", selectedBudget);
                console.log("New Budget: ", newBudget);

                // Вызов API для обновления бюджета
                fetch(`api/Budget/${selectedBudget.key}`, {
                    method: 'PUT',
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify({
                        id: newBudget.key,
                        startDate: newBudget.startDate,
                        timePeriodId: newBudget.timePeriodId,
                        userId: newBudget.userId,
                        properties: ""
                    }),
                })
                    .then((response) => {
                        if (response.status === 204) {

                            // Обновление списка бюджетов после успешного обновления
                            fetch(`api/TimePeriod/${newBudget.timePeriodId}`)
                                .then((timePeriodResponse) => {
                                    if (timePeriodResponse.status === 200) {
                                        return timePeriodResponse.json();
                                    } else {
                                        throw new Error("Failed to fetch time period");
                                    }
                                })
                                .then((timePeriodData) => {
                                    newBudget.timePeriod = timePeriodData.name;
                                    setBudgets((prevBudgets) => 
                                        prevBudgets.map((budget) => 
                                            budget.key === selectedBudget.key ? newBudget : budget
                                        )
                                    );
                                })
                                .catch((error) => {
                                    console.log(error);
                                });
                                
                                
                                Promise.all(newListOfPlannedExpenses.map((plannedExpenses) =>
                                    fetch(`api/PlannedExpenses/${plannedExpenses.id}`, {
                                        method: "PUT",
                                        headers: {
                                            "Content-Type": "application/json",
                                        },
                                        body: JSON.stringify({
                                            id: plannedExpenses.id,
                                            expenseTypeId: plannedExpenses.expenseTypeId,
                                            sum: plannedExpenses.sum,
                                            budgetId: plannedExpenses.budgetId,
                                            expenseType: ""
                                        }),
                                    })
                                ))
                                        .then((expenseResponses) => {
                                            // Обработка ответов на создание запланированных расходов
                                            if (expenseResponses.status === 204) 
                                            {
                                                // Обработка ответов на создание запланированных расходов
                                                const updatedListOfPlannedExpenses = listOfPlannedExpenses.map((plannedExpense) => {
                                                    const matchingExpense = newListOfPlannedExpenses.find((newPlannedExpense) => newPlannedExpense.id === plannedExpense.id);
                                                    if (matchingExpense) {
                                                        return {
                                                            ...plannedExpense,
                                                            expenseTypeId: matchingExpense.expenseTypeId,
                                                            sum: matchingExpense.sum,
                                                            budgetId: matchingExpense.budgetId
                                                        };
                                                    }
                                                    return plannedExpense;
                                                });
                                                
                                                setListOfPlannedExpenses(updatedListOfPlannedExpenses);
                                                console.log("Updated List of Planned Expenses: ", updatedListOfPlannedExpenses);
                                            }
                                        })
                                        .catch((error) => {
                                            console.log("Failed to update planned expenses:", error);
                                        });


                                Promise.all(newListOfPlannedIncomes.map((plannedIncomes) =>
                                    fetch(`api/plannedIncomes/${plannedIncomes.id}`, {
                                        method: "PUT",
                                        headers: {
                                            "Content-Type": "application/json",
                                        },
                                        body: JSON.stringify({
                                            id: plannedIncomes.id,
                                            incomeTypeId: plannedIncomes.incomeTypeId,
                                            sum: plannedIncomes.sum,
                                            budgetId: plannedIncomes.budgetId,
                                            incomeType: ""
                                        }),
                                    })
                                ))
                                        .then((incomeResponses) => {
                                            // Обработка ответов на создание запланированных расходов
                                            if (incomeResponses.status === 204) 
                                            {
                                                // Обработка ответов на создание запланированных расходов
                                                const updatedListOfPlannedIncomes = listOfPlannedIncomes.map((plannedIncome) => {
                                                    const matchingIncome = newListOfPlannedIncomes.find((newPlannedIncome) => newPlannedIncome.id === plannedIncome.id);
                                                    if (matchingIncome) {
                                                        return {
                                                            ...plannedIncome,
                                                            incomeTypeId: matchingIncome.incomeTypeId,
                                                            sum: matchingIncome.sum,
                                                            budgetId: matchingIncome.budgetId
                                                        };
                                                    }
                                                    return plannedIncome;
                                                });
                                                
                                                setListOfPlannedIncomes(updatedListOfPlannedIncomes);
                                                console.log("Updated List of Planned Incomes: ", updatedListOfPlannedIncomes);
                                            }
                                        })
                                        .catch((error) => {
                                            console.log("Failed to update planned incomes:", error);
                                        });                             
                                
                        } else {
                            throw new Error("Failed to update budget");
                        }
                    })
                    .catch((error) => {
                        console.log(error);
                    })
            } else {

                // Вызов API для создания бюджета
                fetch("api/Budget", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify({
                        startDate: newBudget.startDate,
                        timePeriodId: newBudget.timePeriodId,
                        userId: newBudget.userId,
                        properties: ""
                    }),
                })
                    .then((response) => {
                        if(response.ok) {
                            return response.json();
                        } else {
                            throw new Error("Failed to create budget");
                        }
                    })
                    .then((createdBudget) => {
                        console.log("New Budget: ", newBudget);

                        // Обновление списка бюджетов после успешного создания
                        fetch(`api/TimePeriod/${newBudget.timePeriodId}`)
                            .then((timePeriodResponse) => {
                                if (timePeriodResponse.status === 200) {
                                    return timePeriodResponse.json();
                                } else {
                                    throw new Error("Failed to fetch time period");
                                }
                            })
                            .then((timePeriodData) => {
                                newBudget.timePeriod = timePeriodData.name;

                                fetch(`api/Budget/user/${userId}`)
                                    .then((allBudgetsResponse) => {
                                        if (allBudgetsResponse.status === 200)
                                        {
                                            return allBudgetsResponse.json();
                                        }
                                        else {
                                            throw new Error("Failed to fetch allBudgets after creating budget");
                                        }
                                    })
                                    .then((allBudgetsData) => {
                                        newBudget.key = allBudgetsData[allBudgetsData.length - 1].id;
                                        console.log(newBudget);


                                        // Обновление списка запланированных расходов с учетом нового budgetId
                                        const newListOfPlannedExpenses = expenseTypes.map((expenseType) => ({
                                            expenseTypeId: expenseType.id,
                                            sum: values[`expense_${expenseType.id}`] || 0,
                                            budgetId: newBudget.key,
                                        }));
                                
                                        Promise.all(newListOfPlannedExpenses.map((plannedExpenses) =>
                                            fetch("api/PlannedExpenses", {
                                                method: "POST",
                                                headers: {
                                                    "Content-Type": "application/json",
                                                },
                                                body: JSON.stringify({
                                                    expenseTypeId: plannedExpenses.expenseTypeId,
                                                    sum: plannedExpenses.sum,
                                                    budgetId: plannedExpenses.budgetId,
                                                    expenseType: ""
                                                }),
                                            })
                                        ))
                                                .then((expenseResponses) => {
                                                    // Обработка ответов на создание запланированных расходов
                                                   
                                                    fetch(`api/PlannedExpenses/budget/${newBudget.key}`)
                                                        .then((allBudgetsPlannedExpensesResponse) => {
                                                            if (allBudgetsPlannedExpensesResponse.status === 200)
                                                            {
                                                                return allBudgetsPlannedExpensesResponse.json();
                                                            }
                                                            else {
                                                                throw new Error("Failed to fetch allBudgetsPlannnedExpenses after creating plannedExpenses");
                                                            }
                                                        })
                                                        .then((allBudgetsPlannedExpensesData) => {
                                                            const lastExpenseIndex = allBudgetsPlannedExpensesData[allBudgetsPlannedExpensesData.length - 1].id;
                                                            console.log("lastExpenseIndex", lastExpenseIndex);
                                                            
                                                            for (let i = lastExpenseIndex - 15, j = 0; i <= lastExpenseIndex; i++, j++) {
                                                                newListOfPlannedExpenses[j].id = i; // Assign new id
                                                            }
                                                            console.log("New List Of PlannedExpenses: ", newListOfPlannedExpenses);
                                                        })

                                                    console.log("Planned expenses created:", expenseResponses);
                                                })
                                                .catch((error) => {
                                                    console.log("Failed to create planned expenses:", error);
                                                });

                                        // Обновление списка запланированных доходов с учетом нового budgetId
                                        const newListOfPlannedIncomes = incomeTypes.map((incomeType) => ({
                                            incomeTypeId: incomeType.id,
                                            sum: values[`income_${incomeType.id}`] || 0,
                                            budgetId: newBudget.key
                                        }));
                                        console.log(newListOfPlannedIncomes); 
                                        
                                        Promise.all(newListOfPlannedIncomes.map((plannedIncomes) =>
                                        fetch("api/plannedIncomes", {
                                            method: "POST",
                                            headers: {
                                                "Content-Type": "application/json",
                                            },
                                            body: JSON.stringify({
                                                incomeTypeId: plannedIncomes.incomeTypeId,
                                                sum: plannedIncomes.sum,
                                                budgetId: plannedIncomes.budgetId,
                                                incomeType: ""
                                            }),
                                        })
                                        ))
                                            .then((incomeResponses) => {
                                                // Обработка ответов на создание запланированных доходов
                                               
                                                fetch(`api/PlannedIncomes/budget/${newBudget.key}`)
                                                    .then((allBudgetsPlannedIncomesResponse) => {
                                                        if (allBudgetsPlannedIncomesResponse.status === 200)
                                                        {
                                                            return allBudgetsPlannedIncomesResponse.json();
                                                        }
                                                        else {
                                                            throw new Error("Failed to fetch allBudgetsPlannedIncomesResponse after creating plannedIncomes");
                                                        }
                                                    })
                                                    .then((allBudgetsPlannedIncomesData) => {
                                                        const lastIncomeIndex = allBudgetsPlannedIncomesData[allBudgetsPlannedIncomesData.length - 1].id;
                                                        console.log("lastIncomeIndex", lastIncomeIndex);
                                                        
                                                        for (let i = lastIncomeIndex - 7, j = 0; i <= lastIncomeIndex; i++, j++) {
                                                            newListOfPlannedIncomes[j].id = i; // Assign new id
                                                        }
                                                        console.log("New List Of PlannedIncomes: ", newListOfPlannedIncomes);
                                                    })


                                                console.log("Planned incomes created:", incomeResponses);
                                            })
                                            .catch((error) => {
                                                console.log("Failed to create planned incomes:", error);
                                            });
                                        
                                    })
                                    .catch((error) => {
                                        console.log(error);
                                    })
                                    setBudgets((prevBudgets) => [...prevBudgets, newBudget]);
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
            setSelectedTimePeriod(null);
            setModalVisible(false);
        });
    };

    const columns = [
        { title: "Начальная дата", dataIndex: "startDate"},
        { title: "Временной период", dataIndex: "timePeriod"},
        {
            render: (_, budget) => (
                <>
                    <Button onClick={() => handleEditBudget(budget)}>Подробнее</Button>
                    <Button onClick={() => handleDeleteBudget(budget.key)}>Удалить</Button>
                </>
            ),
        },
    ];

    const selectedStartDate = selectedBudget ? moment(selectedBudget.startDate) : null;

    return (
        <>
            <Button onClick={handleAddBudget} style={{ marginBottom: 16 }} className="addButton">
                Добавить бюджет
            </Button>
        
            <Table dataSource={budgets} columns={columns} />
            <Modal
                destroyOnClose={true}
                title={selectedBudget ? "Редактирование бюджета" : "Добавление бюджета"}
                open={modalVisible}
                onOk={handleModalOk}
                onCancel={handleModelCancel}
                width={790}
            >
                <Form form={form} layout="vertical">
                    <Row gutter={16}>
                        <Col span={12}>
                            <Form.Item
                                name="startDate"
                                label="Начальная дата"
                                initialValue={selectedStartDate}
                                rules={[{ required: true, message: "Введите начальную дату" }]}
                            >
                            <DatePicker format="YYYY-MM-DD" />
                            </Form.Item>
                        </Col>
                        <Col span={12}>
                            <Form.Item
                                name="timePeriodId"
                                label="Временной период"
                                initialValue={selectedTimePeriod}
                                rules={[{ required: true, message: "Выберите временной период" }]}
                            >
                                <Select
                                    showSearch
                                    placeholder="Выберите временной период"
                                    optionFilterProp="children"
                                    onChange={setSelectedTimePeriod}
                                    filterOption={(input, option) =>
                                        option.children.toLowerCase().indexOf(input.toLowerCase()) >= 0
                                    }
                                >
                                    {timePeriods.map((timePeriod) => (
                                    <Select.Option key={timePeriod.id} value={timePeriod.id}>
                                        {timePeriod.name}
                                    </Select.Option>
                                    ))}
                                </Select>
                            </Form.Item>
                        </Col>
                    </Row>
                    <Row gutter={16}>
                        <Col span={9}>
                            <h3>Запланированные доходы</h3>
                            {incomeTypes.map((incomeType) => (
                                <Form.Item 
                                    key={incomeType.id}
                                    label={incomeType.name}
                                    initialValue={ 
                                        listOfPlannedIncomes.length > 0 &&
                                        typeof listOfPlannedIncomes.find((income) => income.incomeTypeId === incomeType.id)?.sum !== 'undefined'
                                        ? listOfPlannedIncomes.find((income) => income.incomeTypeId === incomeType.id).sum
                                        : 0
                                    }
                                    name={`income_${incomeType.id}`}
                                >
                                    <Input type="number" step="1"/>
                                </Form.Item>
                            ))}
                        </Col>
                        <Col span={12}>
                        <h3>Запланированные расходы</h3>
                            <Row gutter={16}>
                                <Col span={12}>
                                    {expenseTypes.slice(0, Math.ceil(expenseTypes.length / 2)).map((expenseType) => (
                                        <Form.Item 
                                            key={expenseType.id}
                                            label={expenseType.name}
                                            initialValue={             
                                                listOfPlannedExpenses.length > 0 &&
                                                typeof listOfPlannedExpenses.find((expense) => expense.expenseTypeId === expenseType.id)?.sum !== 'undefined'
                                                ? listOfPlannedExpenses.find((expense) => expense.expenseTypeId === expenseType.id).sum
                                                : 0
                                            }
                                            name={`expense_${expenseType.id}`}
                                        >
                                            <Input type="number" step="1"/>
                                        </Form.Item>
                                    ))}
                                </Col>
                                <Col span={12}>
                                    {expenseTypes.slice(Math.ceil(expenseTypes.length / 2)).map((expenseType) => (
                                        <Form.Item 
                                            key={expenseType.id}
                                            label={expenseType.name}
                                            initialValue={
                                                listOfPlannedExpenses.length > 0 &&
                                                typeof listOfPlannedExpenses.find((expense) => expense.expenseTypeId === expenseType.id)?.sum !== 'undefined'
                                                ? listOfPlannedExpenses.find((expense) => expense.expenseTypeId === expenseType.id).sum
                                                : 0
                                            }
                                            name={`expense_${expenseType.id}`}
                                        >
                                            <Input type="number" step="1"/>
                                        </Form.Item>
                                    ))}
                                </Col>
                            </Row>
                        </Col>
                    </Row>
                </Form>
            </Modal>
        </>
    );

};

export default Budgets;