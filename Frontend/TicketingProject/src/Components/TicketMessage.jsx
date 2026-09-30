import { useEffect, useState } from "react";
import TicketCard from "./TicketCard";
import { fetchWithAuth } from "../APIhelper/FetchWithAuth";

function TicketMessage({ticket_detail}){
    const [userid,setuserid]  = useState(null);
    const getuserid = async()=>{
        try{
            const response = await fetchWithAuth("/api/Login/getuserid",{
                method:"GET"
            })
            const data = await response.json();
            setuserid(data.id);
        }catch(error){
            console.log(`theres an error${error}`);
        }
    }
    useEffect(()=>{
        getuserid();
    },[])
    if (!ticket_detail) return <p className="loading">Loading...</p>;
    return(
        <div className="ticket-message-wrapper">
            <TicketCard ticket={ticket_detail.ticket_detail}></TicketCard>
            <div className="chat-container">
                {ticket_detail.ticket_detail.messages.map((mess, index) => {
                    const isMine = mess.senderId === userid;
                    return (
                        <div
                            key={index}
                            className={`message ${isMine ? "right" : "left"}`}>
                            <p>{mess.message}</p>
                            <small>{mess.dateIn}</small>
                        </div>
                    );
                })}
            </div>
        </div>
    )
}

export default TicketMessage;