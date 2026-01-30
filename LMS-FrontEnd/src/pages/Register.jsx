// import { useState } from "react";
// import { useNavigate, Link } from "react-router-dom";
// import api from "../services/api";
// import { isStrongPassword, isValidEmail } from "../utils/validators";

// export default function Register() {
//   const nav = useNavigate();
//   const [showPass, setShowPass] = useState(false);

//   const [form, setForm] = useState({
//     firstName: "", lastName: "", email: "", username: "", mobileNumber: "",
//     password: "", dateOfBirth: "", address: "", bio: "",
//     role: "Student", highestEducation: ""
//   });

//   const [msg, setMsg] = useState("");

//   const handleChange = (e) => setForm({ ...form, [e.target.name]: e.target.value });

//   const submit = async (e) => {
//   e.preventDefault();
//   setMsg("");

//   if (!isValidEmail(form.email)) return setMsg("Invalid email format.");
//   if (!isStrongPassword(form.password))
//     return setMsg("Password must be 8+ chars with Upper, Lower, Number, Special.");

//   if (form.role === "Mentor" && !form.highestEducation.trim())
//     return setMsg("Highest education is required for Mentor.");

//   // ✅ CREATE payload properly
//   const payload = {
//     firstName: form.firstName,
//     lastName: form.lastName,
//     email: form.email,
//     username: form.username,
//     mobileNumber: form.mobileNumber,
//     password: form.password,
//     dateOfBirth: form.dateOfBirth || null,
//     address: form.address,
//     bio: form.bio,
//     highestEducation: form.highestEducation,
//     role: form.role === "Admin" ? 1 : form.role === "Mentor" ? 2 : 3
//   };

//   console.log("✅ Register Payload:", payload);

//   try {
//     const res = await api.post("/auth/register", payload);
//     console.log("✅ Register API Response:", res.data);

//     alert("Registration Successful ✅ Redirecting to Login...");
//     nav("/login");
//   } catch (err) {
//     console.log("❌ Register Error:", err);
//     setMsg(err?.response?.data || err?.message || "Registration failed.");
//   }
// };

//   return (
//     <div className="container py-5" style={{ maxWidth: 800 }}>
//       <div className="card shadow-sm p-4">
//         <h3 className="fw-bold">Registration</h3>
//         <p className="text-muted">Create your account</p>

//         {msg && <div className="alert alert-danger">{msg}</div>}

//         <form onSubmit={submit}>
//           <div className="row g-3">
//             <div className="col-md-6">
//               <label className="form-label">First Name</label>
//               <input className="form-control" name="firstName" value={form.firstName} onChange={handleChange} required />
//             </div>

//             <div className="col-md-6">
//               <label className="form-label">Last Name</label>
//               <input className="form-control" name="lastName" value={form.lastName} onChange={handleChange} required />
//             </div>

//             <div className="col-md-6">
//               <label className="form-label">Email</label>
//               <input className="form-control" name="email" value={form.email} onChange={handleChange} required />
//             </div>

//             <div className="col-md-6">
//               <label className="form-label">Username</label>
//               <input className="form-control" name="username" value={form.username} onChange={handleChange} required />
//             </div>

//             <div className="col-md-6">
//               <label className="form-label">Mobile</label>
//               <input className="form-control" name="mobileNumber" value={form.mobileNumber} onChange={handleChange} required />
//             </div>

//             <div className="col-md-6">
//               <label className="form-label">Role</label>
//               <select className="form-select" name="role" value={form.role} onChange={handleChange}>
//                 <option>Student</option>
//                 <option>Mentor</option>
//                 <option>Admin</option>
//               </select>
//             </div>

//             {form.role === "Mentor" && (
//               <div className="col-md-12">
//                 <label className="form-label">Highest Education</label>
//                 <input className="form-control" name="highestEducation" value={form.highestEducation} onChange={handleChange} />
//               </div>
//             )}

//             <div className="col-md-6">
//               <label className="form-label">Password</label>
//               <div className="input-group">
//                 <input
//                   className="form-control"
//                   type={showPass ? "text" : "password"}
//                   name="password"
//                   value={form.password}
//                   onChange={handleChange}
//                   required
//                 />
//                 <button type="button" className="btn btn-outline-secondary" onClick={() => setShowPass(!showPass)}>
//                   {showPass ? "Hide" : "Show"}
//                 </button>
//               </div>
//             </div>

//             <div className="col-md-6">
//               <label className="form-label">Date of Birth</label>
//               <input type="date" className="form-control" name="dateOfBirth" value={form.dateOfBirth} onChange={handleChange} />
//             </div>

//             <div className="col-md-12">
//               <label className="form-label">Address</label>
//               <input className="form-control" name="address" value={form.address} onChange={handleChange} />
//             </div>

//             <div className="col-md-12">
//               <label className="form-label">Bio</label>
//               <textarea className="form-control" rows="3" name="bio" value={form.bio} onChange={handleChange}></textarea>
//             </div>
//           </div>

//           <button type="submit" className="btn btn-dark w-100 mt-4">Register</button>

//           <div className="text-center mt-3">
//             <span className="text-muted">Already have an account? </span>
//             <Link to="/login" className="text-decoration-none">Login</Link>
//           </div>
//         </form>
//       </div>
//     </div>
//   );
// }

// import { useState } from "react";
// import { useNavigate } from "react-router-dom";
// import api from "../services/api";

// export default function Register() {
//   const navigate = useNavigate();

//   const [fullName, setFullName] = useState("");
//   const [email, setEmail] = useState("");
//   const [password, setPassword] = useState("");
//   const [role, setRole] = useState("Student");
//   const [dateOfBirth, setDateOfBirth] = useState("");
//   const [error, setError] = useState("");

//   const calculateAge = (dob) => {
//     const today = new Date();
//     const birthDate = new Date(dob);
//     let age = today.getFullYear() - birthDate.getFullYear();
//     const m = today.getMonth() - birthDate.getMonth();
//     if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
//       age--;
//     }
//     return age;
//   };

//   const handleSubmit = async (e) => {
//     e.preventDefault();
//     setError("");

//     const age = calculateAge(dateOfBirth);

//     if (role === "Student" && age < 10) {
//       setError("Student must be at least 10 years old");
//       return;
//     }

//     if (role === "Mentor" && age < 21) {
//       setError("Mentor must be at least 21 years old");
//       return;
//     }

//     try {
//       await api.post("/Auth/register", {
//         fullName,
//         email,
//         password,
//         role,
//         dateOfBirth,
//       });

//       navigate("/login");
//     } catch (err) {
//       setError(err.response?.data || "Registration failed");
//     }
//   };

//   return (
//     <div className="max-w-md mx-auto mt-10 p-6 border rounded">
//       <h2 className="text-2xl font-bold mb-4">Register</h2>

//       {error && <p className="text-red-500 mb-3">{error}</p>}

//       <form onSubmit={handleSubmit} className="space-y-3">
//         <input
//           type="text"
//           placeholder="Full Name"
//           value={fullName}
//           onChange={(e) => setFullName(e.target.value)}
//           required
//           className="w-full border p-2 rounded"
//         />

//         <input
//           type="email"
//           placeholder="Email"
//           value={email}
//           onChange={(e) => setEmail(e.target.value)}
//           required
//           className="w-full border p-2 rounded"
//         />

//         <input
//           type="password"
//           placeholder="Password"
//           value={password}
//           onChange={(e) => setPassword(e.target.value)}
//           required
//           className="w-full border p-2 rounded"
//         />

//         <select
//           value={role}
//           onChange={(e) => setRole(e.target.value)}
//           className="w-full border p-2 rounded"
//         >
//           <option value="Student">Student</option>
//           <option value="Mentor">Mentor</option>
//         </select>

//         <input
//           type="date"
//           value={dateOfBirth}
//           onChange={(e) => setDateOfBirth(e.target.value)}
//           required
//           className="w-full border p-2 rounded"
//         />

//         <button
//           type="submit"
//           className="w-full bg-blue-600 text-white p-2 rounded"
//         >
//           Register
//         </button>
//       </form>
//     </div>
//   );
// }


import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import api from "../services/api";
import { isStrongPassword, isValidEmail } from "../utils/validators";

export default function Register() {
  const nav = useNavigate();
  const [showPass, setShowPass] = useState(false);

  const [form, setForm] = useState({
    firstName: "",
    lastName: "",
    email: "",
    username: "",
    mobileNumber: "",
    password: "",
    dateOfBirth: "",
    address: "",
    bio: "",
    role: "Student",
    highestEducation: ""
  });

  const [msg, setMsg] = useState("");

  const handleChange = (e) =>
    setForm({ ...form, [e.target.name]: e.target.value });

  // ✅ Calculate age based on date of birth
  const calculateAge = (dob) => {
    const today = new Date();
    const birthDate = new Date(dob);
    let age = today.getFullYear() - birthDate.getFullYear();
    const m = today.getMonth() - birthDate.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
      age--;
    }
    return age;
  };

  const submit = async (e) => {
    e.preventDefault();
    setMsg("");

    // ✅ Validate email
    if (!isValidEmail(form.email)) return setMsg("Invalid email format.");

    // ✅ Validate password strength
    if (!isStrongPassword(form.password))
      return setMsg(
        "Password must be 8+ chars with Uppercase, Lowercase, Number, and Special character."
      );

    // ✅ Validate DOB & age based on role
    if (!form.dateOfBirth) return setMsg("Date of Birth is required.");
    const age = calculateAge(form.dateOfBirth);
    if (form.role === "Student" && age < 10)
      return setMsg("Student must be at least 10 years old.");
    if (form.role === "Mentor" && age < 21)
      return setMsg("Mentor must be at least 21 years old.");

    // ✅ Validate highest education for Mentor
    if (form.role === "Mentor" && !form.highestEducation.trim())
      return setMsg("Highest education is required for Mentor.");

    // ✅ Build payload
    const payload = {
      firstName: form.firstName,
      lastName: form.lastName,
      email: form.email,
      username: form.username,
      mobileNumber: form.mobileNumber,
      password: form.password,
      dateOfBirth: form.dateOfBirth,
      address: form.address,
      bio: form.bio,
      highestEducation: form.highestEducation,
      role: form.role === "Admin" ? 1 : form.role === "Mentor" ? 2 : 3
    };

    try {
      await api.post("/auth/register", payload);
      alert("✅ Registration Successful. Redirecting to Login...");
      nav("/login");
    } catch (err) {
      setMsg(err?.response?.data || err?.message || "Registration failed.");
    }
  };

  return (
    <div className="container py-5" style={{ maxWidth: 800 }}>
      <div className="card shadow-sm p-4">
        <h3 className="fw-bold">Registration</h3>
        <p className="text-muted">Create your account</p>

        {msg && <div className="alert alert-danger">{msg}</div>}

        <form onSubmit={submit}>
          <div className="row g-3">
            {/* First & Last Name */}
            <div className="col-md-6">
              <label className="form-label">First Name</label>
              <input
                className="form-control"
                name="firstName"
                value={form.firstName}
                onChange={handleChange}
                required
              />
            </div>

            <div className="col-md-6">
              <label className="form-label">Last Name</label>
              <input
                className="form-control"
                name="lastName"
                value={form.lastName}
                onChange={handleChange}
                required
              />
            </div>

            {/* Email & Username */}
            <div className="col-md-6">
              <label className="form-label">Email</label>
              <input
                className="form-control"
                name="email"
                type="email"
                value={form.email}
                onChange={handleChange}
                required
              />
            </div>

            <div className="col-md-6">
              <label className="form-label">Username</label>
              <input
                className="form-control"
                name="username"
                value={form.username}
                onChange={handleChange}
                required
              />
            </div>

            {/* Mobile & Role */}
            <div className="col-md-6">
              <label className="form-label">Mobile</label>
              <input
                className="form-control"
                name="mobileNumber"
                value={form.mobileNumber}
                onChange={handleChange}
                required
              />
            </div>

            <div className="col-md-6">
              <label className="form-label">Role</label>
              <select
                className="form-select"
                name="role"
                value={form.role}
                onChange={handleChange}
              >
                <option>Student</option>
                <option>Mentor</option>
                <option>Admin</option>
              </select>
            </div>

            {/* Highest Education for Mentor */}
            {form.role === "Mentor" && (
              <div className="col-md-12">
                <label className="form-label">Highest Education</label>
                <input
                  className="form-control"
                  name="highestEducation"
                  value={form.highestEducation}
                  onChange={handleChange}
                  required
                />
              </div>
            )}

            {/* Password & DOB */}
            <div className="col-md-6">
              <label className="form-label">Password</label>
              <div className="input-group">
                <input
                  className="form-control"
                  type={showPass ? "text" : "password"}
                  name="password"
                  value={form.password}
                  onChange={handleChange}
                  required
                />
                <button
                  type="button"
                  className="btn btn-outline-secondary"
                  onClick={() => setShowPass(!showPass)}
                >
                  {showPass ? "Hide" : "Show"}
                </button>
              </div>
            </div>

            <div className="col-md-6">
              <label className="form-label">Date of Birth</label>
              <input
                type="date"
                className="form-control"
                name="dateOfBirth"
                value={form.dateOfBirth}
                onChange={handleChange}
                required
              />
            </div>

            {/* Address & Bio */}
            <div className="col-md-12">
              <label className="form-label">Address</label>
              <input
                className="form-control"
                name="address"
                value={form.address}
                onChange={handleChange}
              />
            </div>

            <div className="col-md-12">
              <label className="form-label">Bio</label>
              <textarea
                className="form-control"
                rows="3"
                name="bio"
                value={form.bio}
                onChange={handleChange}
              ></textarea>
            </div>
          </div>

          <button type="submit" className="btn btn-dark w-100 mt-4">
            Register
          </button>

          <div className="text-center mt-3">
            <span className="text-muted">Already have an account? </span>
            <Link to="/login" className="text-decoration-none">
              Login
            </Link>
          </div>
        </form>
      </div>
    </div>
  );
}
