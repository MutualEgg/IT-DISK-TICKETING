import { useState } from "react"
import BackButton from "../../Components/BackButton"
import { useNavigate } from "react-router-dom"
function Login(){
    const navigate = useNavigate();
    const [email,setemail] = useState("")
    const [password,setpassword] = useState("")
    const  SubmitForm = async (e)=>{
        e.preventDefault()
        try{
            const response = await fetch("https://localhost:7055/api/login",{
                method:"POST",
                headers:{
                    "Content-Type":"application/json"
                },
                credentials:"include",
                body:JSON.stringify({
                    email : email,
                    password : password
                })
            })
            const data = await response.json()
            if(data.role == "User"){
                navigate("/usermain");
            }else if(data.role == "Admin"){
                navigate("/adminmain");
            }else if(data.role == "Worker"){
                navigate("/workermain");
            }

        }catch(error){
            console.error("Error:", error)
        }
    }
    return (
        <div className="auth-page">
            <div className="auth-card">
                <h1>Login</h1>
                    <form onSubmit={(e)=>SubmitForm(e)}>
                        <input
                            type="email"
                            placeholder="Email"
                            value={email}
                            required
                            onChange={(e) => setemail(e.target.value)}
                            onInvalid={(e) =>
                                e.target.setCustomValidity(
                                    "Masukkan alamat email yang valid."
                                )
                            }
                            onInput={(e) => e.target.setCustomValidity("")}
                        />
                        <input
                            type="password"
                            placeholder="Password"
                            value={password}
                            required
                            pattern="(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9]).{3,15}"
                            onChange={(e) => setpassword(e.target.value)}
                            onInvalid={(e) =>
                                e.target.setCustomValidity(
                                    "Password harus 3-15 karakter dan mengandung huruf kecil, huruf besar, angka, serta simbol."
                                )
                            }
                            onInput={(e) => e.target.setCustomValidity("")}
                        />
                        <button type="submit">Submit</button>
                    </form>
                    <BackButton/>
            </div>
        </div>
    )
}
export default Login