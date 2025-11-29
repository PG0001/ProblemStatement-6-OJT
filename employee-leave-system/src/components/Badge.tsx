const Badge = ({ status }: { status: string }) => {
const color =
status === "Approved" ? "#16a34a" : status === "Rejected" ? "#dc2626" : "#0ea5e9";
return (
<span style={{ padding: "4px 8px", borderRadius: 6, color: "white", background: color }}>
{status}
</span>
);
};


export default Badge;