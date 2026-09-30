import { useState } from "react"
import { useNavigate } from "react-router-dom";

function AdminNavBar({category,setcategory}){
    const [opencat,setopencat] = useState(false);
    const navigate = useNavigate();
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
        if(cat == "Pick A Cat"){
            setcategory("");
            setopencat(!opencat)
            return;
        }
        setcategory(cat);
        setopencat(!opencat);
    }
    const gotohistory = ()=>{
        //if sudah di history, make ignore aja supaya web tidak load ticket terus menerus
        navigate("/admin/history");
    }
    const gotohome = ()=>{
        navigate("/adminmain")
    }
return(
    <nav className="navbar">
        <div className="navbar-brand">IT Service Desk</div>

        <div className="navbar-links">
            <div className="navbar-item" onClick={gotohome}>Home</div>
            <div className="navbar-item" onClick={gotohistory}>History</div>

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
export default AdminNavBar