import React from "react"
import { Outlet, Link } from "react-router-dom"
import { Layout as LayoutAntd, Menu } from "antd"

import './styles.css';

const { Header, Content, Footer } = LayoutAntd

const Layout = ({ user }) => {
  
    const menuItems = [];
    
    if (!user.isAuthenticated) {
        menuItems.push(
            {
                label: <Link to={"/"}>Стартовое меню</Link>,
                key: "1",
            },
            {
                label: <Link to={"/register"}>Регистрация</Link>,
                key: "2",
            },
            {
                label: <Link to={"/login"}>Войти</Link>,
                key: "3",
            }
        );
    } 
    else {
        menuItems.push(
            {
                label: <Link to={"/incomes"}>Доходы</Link>,
                key: "4",
            },            
            {
                label: <Link to={"/expenses"}>Расходы</Link>,
                key: "5",
            },
            {
                label: <Link to={"/budgets"}>Бюджеты</Link>,
                key: "6",
            },

            {
                label: <Link to={"/logout"}>Выйти</Link>,
                key: "7",
            }
        );

    }

    return (
        <LayoutAntd>
            <Header style={{ position: "sticky", top: 0, zIndex: 1, width: "100%" }}>
                <div
                    style={{
                        float: "right",
                        color: "rgba(255, 255, 255, 0.65)",
                    }}
                >
                {user.isAuthenticated ? (
                    <strong>{user.userName}</strong>
                    ) : (
                    <strong>Гость</strong>
                    )}
                </div>
                <Menu theme="dark" mode="horizontal" items={menuItems} className="menu" />
            </Header>
            <Content className="site-layout" style={{ padding: "0 50px" }}>
                <Outlet />
            </Content>
            <Footer style={{ textAlign: "center" }}>
                FFA ©2023
            </Footer>
        </LayoutAntd>
    )
}

export default Layout