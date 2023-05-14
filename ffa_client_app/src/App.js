import React, {useEffect, useState} from 'react';
import { BrowserRouter, Route, Routes } from 'react-router-dom'

import Register from "./Components/Register/Register"
import Layout from "./Components/Layout/Layout"
import StartMenu from "./Components/StartMenu/StartMenu"
import LogIn from './Components/LogIn/LogIn'
import LogOut from './Components/LogOut/LogOut'
import Incomes from './Components/Incomes/Incomes'
import Expenses from './Components/Expenses/Expenses'
import Budgets from './Components/Budgets/Budgets'

const App = () => {
    const [user, setUser] = useState({
        isAuthenticated: false,
        userId:"",
        userName: "",
        userRole: ""
    })

    useEffect(() => {

        const getUser = async () => {
        return await fetch('api/account/isAuthenticated')
            .then((response) => {
                if (response.status === 200)
                {
                    return response.json();
                }
            })
            
            .then((data) => {
                if (
                    typeof data !== 'undefined' &&
                    typeof data.userId !== 'undefined' &&
                    typeof data.userName !== 'undefined' &&
                    typeof data.userRole !== 'undefined'
                ) {
                    setUser({isAuthenticated: true, userId: data.userId, userName: data.userName, userRole: data.userRole})
                }
            },
            (error) => {
                console.log(error)
            }      
            )
        }

        getUser()
    }, [])

  return (
    <BrowserRouter>
      <Routes>
        <Route path = "/" element = { <Layout user = {user} /> }>
          <Route index element = {<StartMenu />} />

          <Route
            path="/register"
            element={<Register user={user} setUser={setUser} />}
          />

          <Route
            path="/login"
            element = {<LogIn user={user} setUser={setUser} />} 
          />

          <Route 
            path="/logout" 
            element={<LogOut setUser={setUser} />} 
          />

          <Route 
            path="/incomes" 
            element={<Incomes userId={user.userId} />}
          />
          <Route 
            path="/expenses" 
            element={<Expenses userId={user.userId} />} 
          />
          <Route
            path="/budgets"
            element={<Budgets userId={user.userId} />}
          />
        </Route>
      </Routes>
    </BrowserRouter>
  ) 

}

export default App;
