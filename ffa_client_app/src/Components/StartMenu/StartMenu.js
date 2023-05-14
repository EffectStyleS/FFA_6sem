import { useNavigate } from "react-router-dom"
import { Button } from "antd"

import "./styles.css";

const StartMenu = () => {
    const navigate = useNavigate()

    const handleLoginClick = () => navigate("/login");
    const handleRegistrationClick = () => navigate("/register");

    return (
        <>
            <div class="container">
                <div class="wrapper">
                    <div class="left">
                        <h1 class="title">Family<br />Finance<br />Analysis</h1>
                    </div>
                    <div class="right">
                        <div class="btns">
                            <Button 
                                className="btn" 
                                onClick={() => handleLoginClick()} 
                                style={{ marginBottom: 16 }}
                            >
                                Вход
                            </Button>
                            <Button
                                className="btn" 
                                onClick={() => handleRegistrationClick()}
                                style={{ marginBottom: 16 }}
                            >
                                Регистрация
                            </Button>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
};

export default StartMenu;