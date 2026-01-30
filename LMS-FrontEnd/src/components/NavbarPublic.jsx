// import { Link } from "react-router-dom";

// export default function NavbarPublic() {
//   return (
//     <nav className="navbar navbar-expand-lg navbar-dark bg-dark px-3">
//       <Link className="navbar-brand fw-bold" to="/">LMS</Link>

//       <div className="ms-auto d-flex gap-2">
//         <Link className="btn btn-outline-light" to="/login">Login</Link>
//         <Link className="btn btn-warning" to="/register">Register</Link>
//       </div>
//     </nav>
//   );
// }

import { Link, useNavigate } from "react-router-dom";
import { isLoggedIn, getRole } from "../services/auth";

export default function NavbarPublic() {
  const navigate = useNavigate();
  const loggedIn = isLoggedIn();
  const role = getRole();

  const handleBrandClick = () => {
    if (!loggedIn) {
      navigate("/");
    } else {
      // Redirect based on role
      if (role === "Admin") navigate("/admin/dashboard");
      else if (role === "Mentor") navigate("/mentor/dashboard");
      else navigate("/student/dashboard");
    }
  };

  return (
    <nav className="navbar navbar-expand-lg navbar-dark bg-dark px-3">
      {/* Brand */}
      <span
        className="navbar-brand fw-bold"
        role="button"
        onClick={handleBrandClick}
      >
        LMS
      </span>

      {/* Mobile toggle */}
      <button
        className="navbar-toggler"
        type="button"
        data-bs-toggle="collapse"
        data-bs-target="#publicNavbar"
      >
        <span className="navbar-toggler-icon"></span>
      </button>

      <div className="collapse navbar-collapse" id="publicNavbar">
        <div className="ms-auto d-flex gap-2">
          {!loggedIn ? (
            <>
              <Link className="btn btn-outline-light" to="/login">
                Login
              </Link>
              <Link className="btn btn-warning" to="/register">
                Register
              </Link>
            </>
          ) : (
            <Link className="btn btn-success" to={`/${role.toLowerCase()}/dashboard`}>
              Dashboard
            </Link>
          )}
        </div>
      </div>
    </nav>
  );
}
