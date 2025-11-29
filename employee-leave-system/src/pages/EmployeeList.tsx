import Navbar from "../components/Navbar";
import { useEffect, useState } from "react";
import api from "../api/api";


const EmployeesList = () => {
const [employees, setEmployees] = useState<any[]>([]);


useEffect(() => { load(); }, []);


const load = async () => {
const res = await api.get("/employee");
setEmployees(res.data);
};


return (
<div>
<Navbar />
<div style={{ padding: 20 }}>
<h2>Employees</h2>
<table style={{ width: "100%" }} border={1} cellPadding={8}>
<thead>
<tr>
<th>ID</th>
<th>Name</th>
<th>Email</th>
<th>Role</th>
</tr>
</thead>
<tbody>
{employees.map((e)=> (
<tr key={e.id}>
<td>{e.id}</td>
<td>{e.name}</td>
<td>{e.email}</td>
<td>{e.role}</td>
</tr>
))}
</tbody>
</table>
</div>
</div>
);
};


export default EmployeesList;