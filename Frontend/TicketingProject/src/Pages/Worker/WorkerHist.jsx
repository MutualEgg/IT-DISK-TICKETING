import { useEffect, useState } from "react";
import { fetchWithAuth } from "../../APIhelper/FetchWithAuth";
import TicketCard from "../../Components/TicketCard";
import WorkerNavBar from "../../Components/WorkerNavBar";
import BackButton from "../../Components/BackButton";

function WorkerHist(){
    const [tickets, settickets] = useState([]);

    const loadticket = async ()=>{
        try{
            const response = await fetchWithAuth("/api/Worker/tickethistory",{
                method:"GET"
            });

            const data = await response.json();
            settickets(data.tickethist);
        }catch(error){
            console.log(`error = ${error}`);
        }
    };

    useEffect(()=>{
        loadticket();
    },[]);

    return(
        <div>
            <WorkerNavBar/>

            <h1>History</h1>

            {tickets.length > 0 ? (
                tickets.map((ticket,index)=>{
                    return(
                        <TicketCard
                            key={index}
                            ticket={ticket}
                        />
                    );
                })
            ) : (
                <p>No history yet.</p>
            )}
            <BackButton>Back</BackButton>
        </div>
    );
}

export default WorkerHist
