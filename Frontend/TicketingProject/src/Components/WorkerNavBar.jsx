import { useNavigate } from "react-router-dom"

function WorkerNavBar(){
    const navigate = useNavigate()
    const gotohome = ()=>{
        navigate("/workermain");}
    const gotohist =()=>{
        navigate("/workerhist");}
    return (
        <nav className="navbar">
            <div className="navbar-brand">IT Service Desk</div>

            <div className="navbar-links">
                <div className="navbar-item" onClick={gotohome}>
                    Tasks
                </div>

                <div className="navbar-item" onClick={gotohist}>
                    History
                </div>
            </div>
        </nav>
    )
}
export default WorkerNavBar