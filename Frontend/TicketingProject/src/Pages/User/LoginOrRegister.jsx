import { useNavigate } from 'react-router-dom'

function LoginOrRegister(){
    const navigate = useNavigate()
    return (
        <div className="landing-page">
            <div className="landing-card">
                <h1>Selamat Datang</h1>
                <button onClick={() => navigate("/login")}>Login</button>
                <button onClick={() => navigate("/register")}>Register</button>
            </div>
        </div>
    );
}
export default LoginOrRegister