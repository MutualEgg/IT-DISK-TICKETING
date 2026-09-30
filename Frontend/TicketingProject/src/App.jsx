import { useState } from 'react'
import { BrowserRouter, Routes, Route } from 'react-router-dom'
import LoginOrRegister from './Pages/User/LoginOrRegister'
import Login from './Pages/User/Login'
import Register from './Pages/User/Register'
import UserMainPage from './Pages/User/UserMainPage'
import TicketDetail from './Pages/User/TicketDetail'
import AdminMainPage from './Pages/Admin/AdminMainPage'
import AdminUpdateTicket from './Pages/Admin/AdminUpdateTicket'
import AdminHistoryPage from './Pages/Admin/AdminHistoryPage'
import WorkerMainPage from './Pages/Worker/WorkerMainPage'
import WorkerHist from './Pages/Worker/WorkerHist'
import WorkerTicketDetail from './Pages/Worker/WorkerTicketDetail'
import UserHistory from './Pages/User/UserHistory'
function App() {
  const [count, setCount] = useState(0)
  return (
    <BrowserRouter>
      <Routes>
        <Route path ="/" element={<LoginOrRegister/>}></Route>
        <Route path="/login" element={<Login/>}></Route>
        <Route path='/register' element={<Register/>}></Route>
        <Route path='/usermain' element={<UserMainPage></UserMainPage>}></Route>
        <Route path='/userhistory' element={<UserHistory/>}></Route>
        <Route path="/ticket/:id" element={<TicketDetail/>}/>
        <Route path='/adminmain' element={<AdminMainPage/>}></Route>
        <Route path="/adminupdate/:id" element={<AdminUpdateTicket/>}></Route>
        <Route path="/admin/history" element={<AdminHistoryPage/>}></Route>
        <Route path='/workermain' element={<WorkerMainPage/>}></Route>
        <Route path='/workerhist' element={<WorkerHist/>}></Route>
        <Route path='/workerticketdetail/:ticketid' element={<WorkerTicketDetail/>}></Route>
      </Routes>
    </BrowserRouter>
  )
}

export default App
