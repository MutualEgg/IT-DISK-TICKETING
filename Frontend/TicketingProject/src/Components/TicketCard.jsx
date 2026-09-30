import { useEffect, useState } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";

function TicketCard({ ticket ,onClick,admin}) {
  const [count,setcount] = useState(0)
  const readmessage = ()=>{
    setcount(ticket.messages.filter(x=>x==false).length)
  }
  useEffect(()=>{
    if(admin == null){
        readmessage();
    }

  },[])
  return (
  <div className="ticket-card" onClick={onClick}>
    <h3>{ticket.title}</h3>
    <p>Category: {ticket.category}</p>
    <p>Priority: {ticket.priority}</p>
    <p>Date: {ticket.dateIn}</p>

    {admin == null && count > 0 && (
      <span className="notification-badge">
        {count}
      </span>
    )}
  </div>
  )
}

export default TicketCard;