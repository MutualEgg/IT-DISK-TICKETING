import { useEffect, useState } from "react"
import { fetchWithAuth } from "../../APIhelper/FetchWithAuth"
import UserNavBar from "../../Components/UserNavbar"
import TicketCard from "../../Components/TicketCard";
import { useNavigate } from "react-router-dom";
import BackButton from "../../Components/BackButton";

function UserHistory(){
    const [category,setcategory] = useState("");
    const [ticket,setticket] = useState();
    const navigate = useNavigate()
    const loadticket = async()=>{
        const response = await fetchWithAuth(`/api/Ticket/userhistory/${category}`,{
            method:"GET"
        })
        const data = await response.json()
        setticket(data.ticket_detail)
    }
    useEffect(()=>{
        loadticket();
    },[category])
    if(ticket == null){
        return(
            <div>Loading</div>
        )
    }
    const ClickCard= (id)=>{
        navigate(`/ticket/${id}`);
    }
    return (
        <div>
            <UserNavBar category={category} setcategory={setcategory}></UserNavBar>
            {ticket.map((ticket,index)=>{
                return <TicketCard key={index} ticket={ticket} onClick={()=>ClickCard(ticket.ticketId)}></TicketCard>
            })}
            <BackButton>Back</BackButton>
        </div>
    )
}
export default UserHistory