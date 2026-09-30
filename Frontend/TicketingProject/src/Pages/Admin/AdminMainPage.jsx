import { useEffect, useState } from "react";
import AdminNavBar from "../../Components/AdminNavBar";
import { fetchWithAuth } from "../../APIhelper/FetchWithAuth";
import TicketCard from "../../Components/TicketCard";
import { useNavigate } from "react-router-dom";

function AdminMainPage(){
    const [ongoing_tickets,set_ongoing_tickets] = useState([]);
    const [pending_tickets,set_pending_tickets] = useState([]);
    const [category,setcategory] = useState("");
    const [admin,setadmin] = useState(true);
    const navigate = useNavigate();
    const loadticket =async ()=>{
        try{
            const response = await fetchWithAuth(`/api/Ticket/alltickets/${category}`,{
            method:"GET"
        })
            const data = await response.json();
            set_pending_tickets(data.ticket_pending);
            set_ongoing_tickets(data.ticket_ongoing);
        }catch(error){
            console.log(`error = ${error}`);
        }
    }
    useEffect(()=>{
        loadticket();
    },[category])
    const ClickCard = (ticketid)=>{
        navigate(`/adminupdate/${ticketid}`);
    }
    //disini admin ambil semua ticket yang != finished
    //jika ticket di click maka kita akan masuk ke detail lebih dalam maka admin bisa assing worker
    return (
        <div className="dashboard-page">
            <AdminNavBar
                category={category}
                setcategory={setcategory}
            />

            <main className="dashboard-content">

                <section className="ticket-section">
                    <h1 className="section-heading">Ongoing Tickets</h1>

                    <div className="ticket-grid">
                        {ongoing_tickets.map(ticket => (
                            <TicketCard
                                key={ticket.id}
                                ticket={ticket}
                                admin={admin}
                                onClick={() => ClickCard(ticket.id)}
                            />
                        ))}
                    </div>
                </section>

                <section className="ticket-section">
                    <h1 className="section-heading">Pending Tickets</h1>

                    <div className="ticket-grid">
                        {pending_tickets.map(ticket => (
                            <TicketCard
                                key={ticket.id}
                                ticket={ticket}
                                admin = {admin}
                                onClick={() => ClickCard(ticket.id)}
                            />
                        ))}
                    </div>
                </section>

            </main>
        </div>
    )
}
export default AdminMainPage;