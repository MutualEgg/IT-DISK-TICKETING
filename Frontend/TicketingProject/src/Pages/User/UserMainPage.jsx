import { useNavigate } from "react-router-dom"
import { useState,useEffect, use } from "react";
import TicketForm from "../../Components/TicketForm";
import { fetchWithAuth } from "../../APIhelper/FetchWithAuth";
import TicketCard from "../../Components/TicketCard";
import UserNavBar from "../../Components/UserNavbar";
function UserMainPage(){
    const [showform,setshowform] = useState(false);
    const [ticketsongoing, setticketsongoing] = useState([]);
    const [ticketspending,setticketspending] = useState([]);
    const [category,setcategory] = useState("");
    const navigate = useNavigate();
    const handleCreateTicket = async (datafromchild)=>{
        try{
            await fetchWithAuth("/api/Ticket/create",{
                method:"POST",
                headers:{
                    "Content-Type":"application/json"
                },
                body:JSON.stringify({
                    title : datafromchild.title,
                    description : datafromchild.description,
                    category : datafromchild.category,
                    priority : datafromchild.priority
                })
            })
            loadtickethistory();
        }catch(error){
            console.log(`theres an error= ${error}`);
            if(error.message === "UNAUTHENTICATED"){
                console.log("try to relog");
                navigate("/");}
        }finally{
            setshowform(false);
        }
    }
    const loadtickethistory = async ()=>{
        try{
            const response = await fetchWithAuth(`/api/Ticket/loadhistory/${category}`,{
                method:"GET"
            })
            const data = await response.json();
            setticketsongoing(data.ongoing_ticket_list)
            setticketspending(data.pending_ticket_list)
        }catch(error){
            console.log(`error = ${error}`);
            if (error.message === "UNAUTHENTICATED") {
                console.log("try to relog");
                navigate("/");}
        }
    }
    useEffect(()=>{
        loadtickethistory();
    },[category])

    const handleTicketClick=(ticketid)=>{
        navigate(`/ticket/${ticketid}`)
    }
    return (
        <div className="main-page">
            <div className="page-header">
                <h1>Tiket Saya</h1>
                <button onClick={() => setshowform(true)}>+ Create Ticket</button>
            </div>
            {showform && (
                <div className="modal-overlay" onClick={() => setshowform(false)}>
                    <div className="modal-box" onClick={(e) => e.stopPropagation()}>
                        <TicketForm onSubmit={handleCreateTicket} />
                    </div>
                </div>
            )}
            <UserNavBar category={category} setcategory={setcategory}></UserNavBar>
            <div className="section">
                <h2 className="section-title">Pending</h2>
                {ticketspending.length === 0 ? (
                    <div className="empty-state">Belum ada tiket aktif</div>
                ) : (
                    <div className="ticket-grid">
                        {ticketspending.map((ticket) => (
                            <TicketCard
                                key={ticket.id}
                                ticket={ticket}
                                onClick={() => handleTicketClick(ticket.id)}
                            />
                        ))}
                    </div>
                )}
            </div>
            <div className="section">
                <h2 className="section-title">On Going</h2>
                {ticketsongoing.length === 0 ? (
                    <div className="empty-state">Belum ada tiket aktif</div>
                ) : (
                    <div className="ticket-grid">
                        {ticketsongoing.map((ticket) => (
                            <TicketCard
                                key={ticket.id}
                                ticket={ticket}
                                onClick={() => handleTicketClick(ticket.id)}
                            />
                        ))}
                    </div>
                )}
            </div>
        </div>
    );
}
export default UserMainPage