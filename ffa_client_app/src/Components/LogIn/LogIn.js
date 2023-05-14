import React, { useState, useEffect } from "react"
import { useNavigate } from "react-router-dom"
import { Button, Checkbox, Form, Input, Space } from "antd"

const LogIn = ({user, setUser}) => {
    const [errorMessages, setErrorMessages] = useState([])
    const navigate = useNavigate()
    const [userData, setUserData] = useState([]);


    useEffect(() => 
    {
        if (userData !== null)
        {
            console.log(userData);
            if (userData.length !== 0)
            {
                setUser({isAuthenticated: true, userId: userData.userId, userName: userData.userName, userRole: userData.userRole});
            }
        }
    }, [userData])

    useEffect(() => 
    {
        console.log(user);
        if( typeof user !== "undefined" &&
            user.isAuthenticated === true &&
            user.userId !== '' &&
            typeof user.userId !== 'undefined' &&
            user.userName !== "" &&
            typeof user.userName !== 'undefined' &&
            user.userRole !== "" &&
            typeof user.userRole !== 'undefined'
        )
        {
            navigate("/incomes")
        }
    }, [user])

    const logIn = async (formValues) => {
        setUserData(null);

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                userName: formValues.userName,
                password: formValues.password,
                rememberMe: formValues.rememberMe,
            }),
        }
        return await fetch(
            "api/account/login",
            requestOptions
        )
            .then((response) => {
                if (response.status === 200)
                {
                    return response.json()
                }   
            })
            .then((data) => {
                console.log("Data:", data)

                if (
                    typeof data !== "undefined" &&
                    typeof data.userId !== 'undefined' &&
                    typeof data.userName !== "undefined"
                ) {
                    setUserData(data);
                    //setUser({isAuthenticated: true, userId: data.userId, userName: data.userName, userRole: data.userRole})
                    //navigate("/incomes")
                }
                typeof data !== "undefined" &&
                typeof data.error !== "undefined" &&
                setErrorMessages(data.error)
            },
            (error) => {
                console.log(error)
            }
            )
    }

    const renderErrorMessage = () =>
    errorMessages.map((error, index) => <div key={index}>{error}</div>)

    return (
        <div style={{ display: "flex", justifyContent: "center", alignItems: "center", height: "100vh" }}>
            {typeof user !== "undefined" &&
                user.isAuthenticated === true &&
                user.userId !== '' &&
                typeof user.userId !== 'undefined' &&
                user.userName !== "" &&
                typeof user.userName !== 'undefined' &&
                user.userRole !== "" &&
                typeof user.userRole !== 'undefined'? (
                <h3>Пользователь {user.userName} с ролью {user.userRole} успешно вошел в систему</h3>
            ) : (
            <div style={{ display: "flex", justifyContent: "center", alignItems: "center", height: "100vh" }}>
                <Space direction="vertical" size="large">
                    <div style={{ display: "flex", justifyContent: "center" }}>
                        <h1>Family Finance Analysis</h1>
                    </div>
                    <div style={{ display: "flex", justifyContent: "center" }}>
                        <h3>Вход</h3>
                    </div>
                    <Form
                        onFinish={logIn}
                        name="basic"
                        labelCol={{ span: 16 }}
                        wrapperCol={{ span: 16 }}
                        style={{ maxWidth: 600 }}
                        initialValues={{ remember: false }}
                        onFinishFailed={renderErrorMessage}
                        autoComplete="off"
                    >
                        <Form.Item
                            label="Username"
                            name="userName"
                            labelCol={{ span: 10 }}
                            rules={[
                                { required: true, message: "Please input your username!" },
                            ]}
                        >
                            <Input />
                        </Form.Item>
            
                        <Form.Item
                            label="Password"
                            name="password"
                            labelCol={{ span: 10 }}
                            rules={[
                                { required: true, message: "Please input your password!" },
                            ]}
                        >
                            <Input.Password />
                        </Form.Item>
                        
                        <Form.Item
                            name="rememberMe"
                            valuePropName="checked"
                            wrapperCol={{ offset: 8, span: 16 }}
                            labelCol={{ span: 10 }}
                        >
                            <Checkbox>Remember me</Checkbox>
                            {renderErrorMessage()}
                        </Form.Item>
            
                        <Form.Item wrapperCol={{ offset: 8, span: 16 }}  labelCol={{ span: 10 }}>
                            <Button type="primary" htmlType="submit">
                                Войти
                            </Button>
                        </Form.Item>
                    </Form>
                </Space>
            </div>
          )}
        </div>
    )
}

export default LogIn
