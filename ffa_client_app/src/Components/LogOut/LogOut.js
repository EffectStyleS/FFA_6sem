import React, { useEffect, useState } from "react"
import { useNavigate } from "react-router-dom"
import { Modal } from "antd"

const LogOut = ({ setUser }) => {
  const [open, setOpen] = useState(false)
  const navigate = useNavigate()

  const showModal = () => {
    setOpen(true);
  }

  useEffect(() => {
    showModal()
  }, [])

  const logOff = async (event) => {
    event.preventDefault()

    const requestOptions = {
      method: "POST",
    }
    return await fetch(
        "api/account/logoff",
        requestOptions
    )
      .then((response) => {
        if(response.status === 200)
        {
            setUser({ isAuthenticated: false, userId: "", userName: "", userRole: "" })
            setOpen(false)
            navigate("/")
        }
        })      
  }

  const handleCancel = () => {
    console.log("Clicked cancel button")
    setOpen(false)
  }
    
  return (
    <>
      <Modal title="Title" open={open} onOk={logOff} onCancel={handleCancel}>
        <p>Выполнить выход?</p>
      </Modal>
    </>
    )
    
}

export default LogOut