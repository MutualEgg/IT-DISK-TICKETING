import { useNavigate, useParams } from "react-router-dom"
import { fetchWithAuth } from "../../APIhelper/FetchWithAuth";
import { useEffect, useState } from "react";
import BackButton from "../../Components/BackButton";

function AdminUpdateTicket(){
    //self explanatory
    const {id} = useParams();
    const [title,settitle] = useState("");
    const [description,setdescription] = useState("");
    const [category,setcategory] = useState("")
    const [priority,setpriority] = useState("")
    const [worker,setworker] = useState("")
    const [workerlist,setworkerlist] = useState([]);
    const navigate = useNavigate();
    const [ongoing,setongoing] = useState(false);
    const loadticketdetail = async()=>{
        try{
            const response = await fetchWithAuth(`/api/Ticket/${id}`,{
                method:"GET"       
            })
            const data = await response.json();
            if(data.ticket_detail.progress == "ongoing"){
                setongoing(true);
            }
            settitle(data.ticket_detail.title); 
            setdescription(data.ticket_detail.description);
            setcategory(data.ticket_detail.category);
            setpriority(data.ticket_detail.priority);
            setworker(data.ticket_detail.workerId ?? "");
            getworker(data.ticket_detail.category);
;        }
        catch(error){
            console.log(`error = ${error}`);
        }
    }
    useEffect(()=>{
        loadticketdetail();
    },[id])
    const HandleSubmit = async(e)=>{
        try{
            e.preventDefault()
            await fetchWithAuth("/api/Ticket/assignticket",{
                method:"POST",
                headers:{
                    "Content-Type":"application/json"
                },
                body:JSON.stringify({
                    Title:title,
                    Description: description,
                    Category:category,
                    Priority:priority,
                    WorkerId: worker,
                    TicketId :id
                })
            })
        }catch(error){
            console.log(`error message is ${error}`);
        }finally{
            navigate(-1)
        }
    }
    //ambil worker berdasarkan category dari ticket
    const getworker = async(cat)=>{
        try{
            const response = await fetchWithAuth(`/api/Worker/${cat}`,{
                method:"GET"
            })
            const data = await response.json()
            setworkerlist(data.worker_list)
        }catch(error){
            console.log(`error = ${error}`);
        }
    }
    //deleteticket
    const DeleteTicket = async ()=>{
        try{
            await fetchWithAuth(`/api/Ticket/admindelete/${id}`,{
                method:"DELETE"
            })
            navigate(-1);
        }catch(error){
            console.log(`error = ${error}`);
        }
    }
    return(
        <div className="form-page">
            <div className="form-card">

                <h1>Update Ticket</h1>
                <form onSubmit={(e)=>{HandleSubmit(e)}} className="ticket-form">
                        <input
                        type="text"
                        placeholder="Title"
                        value={title}
                        onChange={(e) => settitle(e.target.value)}
                        required
                        maxLength={30}
                        />

                        <input
                        type="text"
                        placeholder="Description"
                        value={description}
                        onChange={(e) => setdescription(e.target.value)}
                        required
                        maxLength={100}
                        />

                        <select
                        value={category}
                        onChange={(e) => {
                            setcategory(e.target.value)
                            getworker(e.target.value)}}
                        required
                        >
                            <option value="">-- Select Category --</option>
                            <option value="Hardware">Hardware</option>
                            <option value="Software">Software</option>
                            <option value="Network">Network</option>
                            <option value="Account">Account</option>
                            <option value="Access Request">Access Request</option>
                            <option value="Classroom Equipment">Classroom Equipment</option>
                            <option value="Other">Other</option>
                        </select>

                        <select
                        value={priority}
                        onChange={(e) => setpriority(e.target.value)}
                        required
                        >
                            <option value="">Select The Priority</option>
                            <option value="3">High</option>
                            <option value="2">Medium</option>
                            <option value="1">Low</option>
                        </select>
                        <select value={worker}
                        onChange={(e)=>setworker(e.target.value)}>
                            <option value="">Select Worker</option>
                            {workerlist.map(name=>(
                                <option value={name.id} key={name.id}>{name.name}</option>
                            ))}
                        </select>
                        {!ongoing && (
                            <button type="submit">Update</button>
                        )}
                    </form>
 

                <div className="form-actions">
                    <button className="danger-button" onClick={DeleteTicket}>
                        Delete
                    </button>

                    <BackButton />
                </div>

            </div>
        </div>
    )
}
export default AdminUpdateTicket


