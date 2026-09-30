import { useState } from "react";

function TicketForm({onSubmit}){
    const [title,settitle] = useState("");
    const [description,setdescription] = useState("")
    const [category,setcategory] = useState("")
    const [priority,setpriority] = useState()
    const [images, setimages] = useState([]);
    const HandleSubmit = (e)=>{
        e.preventDefault()
        onSubmit({
            title : title,
            description : description,
            category :category,
            priority : Number(priority),
            images : images
        })
    }
    return(
        <div>
            <form className="ticket-form" onSubmit={HandleSubmit}>
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
                onChange={(e) => setcategory(e.target.value)}
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
                <input
                    placeholder="Image"
                    type="file"
                    accept="image/*"
                    multiple
                    onChange={(e) => setimages(Array.from(e.target.files))}
                    />
                <button type="submit">Submit</button>
            </form>
        </div>
    )

}

export default TicketForm;