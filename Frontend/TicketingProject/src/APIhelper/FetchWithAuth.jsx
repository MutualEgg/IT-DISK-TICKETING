import { useNavigate } from "react-router-dom";

const API_URL = "https://localhost:7055";

export async function fetchWithAuth(path, options = {}) {

  // default: kirim cookie
  options.credentials = "include";

  let response = await fetch(`${API_URL}${path}`, options);

  if (response.status !== 401) {
    return response;    // OK atau error lain, serahkan ke pemanggil
  }
  //kalau unauthorize, maka kita panggil refresh untuk update our accesstoken
  // 401 → coba refresh
  const refreshRes = await fetch(`${API_URL}/api/Register/refresh`, {
    method: "POST",
    credentials: "include",
  });
  //kalau refresh gagak, maka harus login ulang
  if (!refreshRes.ok) {
    // refresh gagal → lempar error khusus
    const err = new Error("UNAUTHENTICATED");
    err.status = 401;
    throw err;

  }
  //kalau refresh sukses, maka kita jalankan endpoitnnya sekali lagi
  // refresh sukses → retry sekali
  response = await fetch(`${API_URL}${path}`, options);
  return response;
}