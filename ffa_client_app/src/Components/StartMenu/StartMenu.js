import { Link } from "react-router-dom"

const StartMenu = () => {
    return (
      <>
        <h1>Family Finance Analysis</h1>
        <div>
          {/* <Link to="/login">Вход</Link> */}
          <Link to="/register">Регистрация</Link>
          <br />
          <Link to="/login">Вход</Link>
        </div>
      </>
    );
};

export default StartMenu;