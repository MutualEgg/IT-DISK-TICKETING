import { useEffect, useState } from "react";
import { fetchWithAuth } from "../../APIhelper/FetchWithAuth";
import TicketCard from "../../Components/TicketCard";
import AdminNavBar from "../../Components/AdminNavBar";

function AdminHistoryPage(){
    const [tickethistory,settickethistory] = useState([])
    const [category,setcategory] = useState("");
    const loadhistoryticket =async ()=>{
        try{    
            const response = await fetchWithAuth(`/api/Ticket/adminhistory/${category}`,{
                method:"GET"
            })
            const data = await response.json();
            settickethistory(data.ticket_history);
        }catch(error){
            console.log(`error = ${error}`);
        }
    }
    //setiap kali open this page, kita load histroy ticket
    useEffect(()=>{
        loadhistoryticket();
    },[category])
    //admin navbar bertujuan untuk mengambil nilai category lalu send to loadhistory ticket, jadi loadhistory ticket akan load hist berdasarkan category
    return(
        <div>
            <AdminNavBar category={category} setcategory={setcategory}/>
                <main className="dashboard-content">
                    <section className="ticket-section">
                        <h1 className="section-heading">
                            Ticket History
                        </h1>

                        <div className="ticket-grid">
                            {tickethistory.map((ticket,index) => (
                                <TicketCard
                                    key={index}
                                    ticket={ticket}
                                />
                            ))}
                        </div>
                    </section>
                </main>
        </div>
    )
}   
export default AdminHistoryPage;