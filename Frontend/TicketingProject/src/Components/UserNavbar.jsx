import { useState } from "react"
import { useNavigate, useSearchParams } from "react-router-dom"

function UserNavBar({category,setcategory}){
    const navigate = useNavigate()
    const [opencat,setopencat] = useState(false)
    const gotohistory = ()=>{
        navigate("/userhistory");
    }
    const cat_list = [
        "Pick A Cat",
        "Hardware",
        "Software",
        "Network",
        "Account",
        "Access Request",
        "Classroom Equipment",
        "Other"
    ];
    const pickacat = (cat)=>{
        setopencat(!opencat);
        if(cat == "Pick A Cat"){
            setcategory("");
            return;
        }
        setcategory(cat);
    }
    return(
        <nav className="navbar">
            <div className="navbar-brand">IT Service Desk</div>

            <div className="navbar-links">
                <div className="navbar-item" onClick={gotohistory}>
                    History
                </div>

                <div className="navbar-dropdown">
                    <div
                        className="navbar-item"
                        onClick={() => setopencat(!opencat)}
                    >
                        Category : {category || "All"}
                    </div>

                    {opencat && (
                        <div className="category-menu">
                            {cat_list.map((cat,index)=>(
                                <div
                                    className="category-option"
                                    key={index}
                                    onClick={() => pickacat(cat)}
                                >
                                    {cat}
                                </div>
                            ))}
                        </div>
                    )}
                </div>
            </div>
        </nav>
    )
}
export default UserNavBar