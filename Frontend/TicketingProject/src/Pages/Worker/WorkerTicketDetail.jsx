import { useNavigate, useParams } from "react-router-dom"
import { fetchWithAuth } from "../../APIhelper/FetchWithAuth"
import { useEffect, useState } from "react"
import TicketMessage from "../../Components/TicketMessage"
import BackButton from "../../Components/BackButton"

function WorkerTicketDetail(){
    const {ticketid} = useParams()
    const [ticketdetail,setticketdetail] = useState()
    const [message,setmessage] = useState("")
    const navigate = useNavigate()
    const loadticket = async ()=>{
        try{
            const response = await fetchWithAuth(`/api/Ticket/${ticketid}`,{
                method:"GET"
            })
            setticketdetail(await response.json())
            setreadmessage();
        }catch(error){
            console.log(`error = ${error}`)
        }
    }
    useEffect(()=>{
        loadticket();
    },[ticketid])
    const setreadmessage =async ()=>{
        try{
            await fetchWithAuth(`/api/Ticket/read/${ticketid}`,{
                method:"PUT"
            })
        }catch(error){
            console.log(`error = ${error}`);
        }
    }
    const sendmessage = async ()=>{
        try{
            await fetchWithAuth(`/api/Message`,{
                method:"POST",
                headers:{
                    "Content-Type":"application/json"
                },
                body:JSON.stringify({
                    ticketId:ticketid,
                    message :message
                })
            })
            setmessage("");
        }catch(error){
            console.log(`error = ${error}`)
        }finally{
            loadticket()
        }
    }

    const finishticket = async ()=>{
        try{
            const response = await fetchWithAuth(`/api/Worker/finish/${ticketid}`,{
                method:"PUT"
            })
            const data = await response.json()
            console.log(data)
        }catch(error){
            console.log(`error = ${error}`)
        }finally{
            navigate(-1)
        }
    }

    if(ticketdetail == null){
        return (
            <div>Loading</div>
        )
    }
    return (
        <div className="ticket-detail-page">
            <TicketMessage ticket_detail={ticketdetail} />
            {ticketdetail.ticket_detail.progress !== "finished" && (
                <div className="message-bar">
                    <input
                        type="text"
                        placeholder="Tulis pesan..."
                        value={message}
                        onChange={(e) => {
                            setmessage(e.target.value);
                        }}
                    />

                    <button
                        type="button"
                        onClick={() => sendmessage()}
                        disabled={!message.trim()}
                    >
                        Send Message
                    </button>
                    <button type="button" onClick={()=> finishticket()}>Finish</button>
                </div>
            )}
            <BackButton>Back</BackButton>
        </div>


    )
}

export default WorkerTicketDetail