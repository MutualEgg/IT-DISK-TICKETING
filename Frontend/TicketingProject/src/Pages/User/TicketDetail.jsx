import { useNavigate, useParams } from "react-router-dom"
import { useState,useEffect } from "react";
import { fetchWithAuth } from "../../APIhelper/FetchWithAuth";
import TicketMessage from "../../Components/TicketMessage";
import BackButton from "../../Components/BackButton";

function TicketDetail(){
    const [message,setmessage] = useState("");
    const {id} = useParams();
    const [detail,setdetail] = useState(null);
    const navigate = useNavigate();
    const loadticketdetail = async ()=>{
        try{
            const response = await fetchWithAuth(`/api/Ticket/${id}`,{
                method :"GET"
            })
            setdetail(await response.json())
            setmessagetrue();
        }catch(error){
            console.log(`error = ${error}`);
        }
    }
    const setmessagetrue = async()=>{
        try{
            const response = await fetchWithAuth(`/api/Ticket/read/${id}`,{
                method:"PUT"
            })
        }catch(error){
            console.log(`error = ${error}`);
        }
    }
    useEffect(()=>{
        loadticketdetail();
    },[id])
    const postmessage = async ()=>{
        try{
            console.log(id);
            await fetchWithAuth("/api/Message",{
                method:"POST",
                headers :{
                    "Content-Type":"application/json"
                },
                body : JSON.stringify({
                    message : message,
                    TicketId : id
                })
            })
            setmessage("");           // ← kosongkan input
            await loadticketdetail(); // ← reload ticket detail supaya message baru muncul
        }catch(error){
            console.log(`error message is ${error}`);
            if(error == "UNAUTHORIZE"){
                navigate("/");
                return;
            }
        }
    }
    const deleteticket = async ()=>{
        try{
            await fetchWithAuth(`/api/Ticket/delete/${id}`,{
                method:"DELETE"
            })
            navigate(-1);
        }catch(error){
            console.log(`error is ${error}`);
            if(error== "UNAUTHORIZED"){
                navigate("/");
            }
        }
    }
    if (detail == null) {
    return <div className="loading">Loading...</div>;
    }

    return (
        <div className="ticket-detail-page">
            <TicketMessage ticket_detail={detail} />
            {detail.ticket_detail.progress !== "finished" && (
                <div className="message-bar">
                    <input
                        type="text"
                        placeholder="Tulis pesan..."
                        onChange={(e) => setmessage(e.target.value)}
                        value={message}
                    />
                    <button onClick={postmessage} disabled={!message.trim()}>
                        Kirim
                    </button>
                    <button className="danger" type="button" onClick={deleteticket}>
                        Hapus
                    </button>
                </div>
            )}
            <BackButton>Back</BackButton>
        </div>
    );
}

export default TicketDetail