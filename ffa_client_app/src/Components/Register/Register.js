import React, {useState} from 'react';
import {useNavigate} from 'react-router-dom';
import { Form, Input, Button, Select } from 'antd'



const Register = ({user, setUser}) => {
    const [errorMessages, setErrorMessages] = useState([]);
    const [registrationSuccess, setRegistrationSuccess] = useState(false);
    const [selectedRole, setSelectedRole] = useState('user');

    const navigate = useNavigate();
    const register = async (formValues) => {      
        // const {userName, password, repassword, userRole} = document.forms[0];       
        // console.log(userName.value, password.value)

        const requestOptions = {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify({
                userName: formValues.userName,
                password: formValues.password,
                passwordConfirm: formValues.repassword,
                role: selectedRole
            })
        };
        return await fetch(
            'api/account/register',
            requestOptions,
        )
            .then((response) => {
                console.log(response.status)
                if(response.status === 200)
                {
                    return response.json();
                }
            })
            .then(
                (data) => {
                    console.log("Data:", data)
                    if (
                        typeof data !== 'undefined' &&
                        typeof data.userId !== 'undefined' &&
                        typeof data.userName !== 'undefined' &&
                        typeof data.userRole !== 'undefined'
                    ) {
                        setUser({isAuthenticated: true, userId: data.userId, userName: data.userName, userRole: data.userRole});
                        setRegistrationSuccess(true);
                        navigate('/incomes'); // перенаправлять на главное меню
                    }

                    typeof data !== 'undefined' &&
                    typeof data.error !== 'undefined' &&
                    setErrorMessages(data.error);
                },
                (error) => {
                    console.log(error);
                },
            );
    };

    const renderErrorMessage = () => errorMessages.map((error, index) => <div key={index}>{error}</div>);

    const handleRoleChange = (value) => {
        setSelectedRole(value); // update the selected role
    };

    return (
        <>
        {user.isAuthenticated ? (
            <h3>Пользователь {user.userName} с ролью {user.userRole} уже вошел в систему</h3>
        ): (
            <>
                <h3>Регистрация</h3>
                <Form
                    onFinish={register}
                    name="basic"
                    labelCol={{ span: 8 }}
                    wrapperCol={{ span: 16 }}
                    style={{ maxWidth: 600 }}
                    initialValues={{ role: "user" }}
                    onFinishFailed={renderErrorMessage}
                    autoComplete="off"
                >
                    <Form.Item
                        label="Username"
                        name="userName"
                        rules={[
                            { required: true, message: "Please input your username!" },
                        ]}
                    >
                        <Input />
                    </Form.Item>

                    <Form.Item
                        label="Password"
                        name="password"
                        rules={[
                            { required: true, message: "Please input your password!" },
                        ]}
                    >
                        <Input.Password />
                    </Form.Item>

                    <Form.Item
                        label="Repeat Password"
                        name="repassword"
                        rules={[
                            { required: true, message: "Please repeat your password!" },
                        ]}
                    >
                        <Input.Password />
                    </Form.Item>

                    <Form.Item
                        label="Role"
                        name="role"
                        rules={[
                            { required: true, message: "Please choose your role!" },
                        ]}
                    >
                        <Select
                            style={{ width: 120 }}
                            onChange={handleRoleChange}
                            options = {[
                                { value: "user", label: "User" },
                                { value: "admin", label: "Admin" }
                            ]}
                        />
                    </Form.Item>

                    <Form.Item wrapperCol={{ offset: 8, span: 16 }}>
                        <Button type="primary" htmlType="submit">
                            Зарегистрироваться
                         </Button>
                    </Form.Item>
                </Form>

                {registrationSuccess && (
                    // eslint-disable-next-line max-len
                    <p>Регистрация прошла успешно. Вы будете перенаправлены на главную страницу.</p>
                )}
                {renderErrorMessage()}
            </>
        )}
        </>
    );
};

export default Register;