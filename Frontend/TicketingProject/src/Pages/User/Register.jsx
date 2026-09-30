import { useState } from "react"
import BackButton from "../../Components/BackButton"
import { useNavigate } from "react-router-dom"

function Register(){
    const [username,setusername] = useState("")
    const [password,setpassword] = useState("")
    const [email, setemail] = useState("")
    const navigate = useNavigate()
    const SubmitForm = async (e) => {
        e.preventDefault()
        try {
            const response = await fetch("https://localhost:7055/api/Register", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                    credentials: "include",
                body: JSON.stringify({
                    username: username,
                    password: password,
                    email: email
                })
            })
            if (!response.ok) {
                return;
            }
            navigate("/usermain");
        } catch (error) {
            console.error("Error:", error)
        }
    }
    return(
        <div className="auth-page">
            <div className="auth-card">
                <h1>Register</h1>
                    <form onSubmit={SubmitForm}>
                        <input
                        type="text"
                        placeholder="Username"
                        value={username}
                        required
                        pattern="[A-Za-z0-9 ]{3,15}"
                        onChange={(e) => setusername(e.target.value)}
                        onInvalid={(e) =>
                            e.target.setCustomValidity(
                                "Username harus 3-15 karakter dan hanya boleh mengandung huruf, angka, dan spasi."
                            )
                        }
                        onInput={(e) => e.target.setCustomValidity("")}
                        />

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

export default Register