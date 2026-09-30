import { useEffect, useState } from "react";
import { fetchWithAuth } from "../../APIhelper/FetchWithAuth";
import WorkerNavBar from "../../Components/WorkerNavBar"
import { useNavigate, useSearchParams } from "react-router-dom";
import TicketCard from "../../Components/TicketCard";

function WorkerMainPage(){
    const [tickets,settickets] = useState([])
    const navigate = useNavigate();
    const loadtask = async()=>{
        try{
            const response = await fetchWithAuth("/api/Worker/task",{
                method:"GET"
            })
            const data = await response.json()
            settickets(data.task_tickets);
        }catch(error){
            console.log(`error = ${error}`);
        }
    }
    useEffect(()=>{
        loadtask();
    },[])
    const openTask = (ticketid)=>{
        navigate(`/workerticketdetail/${ticketid}`);
    }
    return(
        <div className="dashboard-page">
            <WorkerNavBar />

            <main className="dashboard-content">
                <section className="ticket-section">

                    <h1 className="section-heading">
                        My Tasks
                    </h1>

                    <div className="ticket-grid">
                        {tickets.map(ticket => (
                            <TicketCard
                                key={ticket.ticketId}
                                ticket={ticket}
                                onClick={() => openTask(ticket.ticketId)}
                            />
                        ))}
                    </div>

                </section>
            </main>
        </div>
    )
}
export default WorkerMainPage