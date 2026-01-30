// import { useEffect, useState } from "react";
// import api from "../../services/api";

// export default function MentorFeedback() {
//   const [feedbacks, setFeedbacks] = useState([]);
//   const [loading, setLoading] = useState(true);

//   useEffect(() => {
//     api.get("/Feedback/mentor")
//       .then(res => setFeedbacks(res.data))
//       .catch(err => console.error(err))
//       .finally(() => setLoading(false));
//   }, []);

//   if (loading) return <p>Loading feedback...</p>;

//   return (
//     <div className="p-6">
//       <h2 className="text-2xl font-bold mb-4">Student Feedback</h2>

//       {feedbacks.length === 0 ? (
//         <p>No feedback available for your courses.</p>
//       ) : (
//         <div className="space-y-4">
//           {feedbacks.map((f, index) => (
//             <div
//               key={index}
//               className="border rounded-lg p-4 shadow-sm bg-white"
//             >
//               <h3 className="font-semibold text-lg">{f.courseTitle}</h3>
//               <p className="text-yellow-600">Rating: {f.rating} ⭐</p>
//               <p className="italic">"{f.comment}"</p>
//               <p className="text-sm text-gray-500">
//                 By {f.studentName} • {new Date(f.createdAt).toLocaleDateString()}
//               </p>
//             </div>
//           ))}
//         </div>
//       )}
//     </div>
//   );
// }


// import { useEffect, useState } from "react";
// import api from "../../services/api";
// import Navbar from "../../components/NavbarPublic.jsx"; // ✅ NAVBAR

// export default function MentorFeedback() {
//   const [feedbacks, setFeedbacks] = useState([]);
//   const [loading, setLoading] = useState(true);
//   const [error, setError] = useState("");

//   const fetchFeedbacks = async () => {
//     try {
//       const res = await api.get("/feedback/mentor");
//       setFeedbacks(res.data);
//     } catch (err) {
//       console.error(err);
//       setError("Failed to load feedback");
//     } finally {
//       setLoading(false);
//     }
//   };

//   useEffect(() => {
//     fetchFeedbacks();
//   }, []);

//   const reportFeedback = async (feedbackId) => {
//     if (!window.confirm("Are you sure you want to report this feedback?"))
//       return;

//     try {
//       await api.post(`/feedback/${feedbackId}/report`);

//       setFeedbacks((prev) =>
//         prev.map((f) =>
//           f.feedbackId === feedbackId
//             ? { ...f, isReported: true }
//             : f
//         )
//       );

//       alert("Feedback reported successfully");
//     } catch {
//       alert("Failed to report feedback");
//     }
//   };

//   if (loading) return <p className="p-4">Loading feedback...</p>;
//   if (error) return <p className="p-4 text-danger">{error}</p>;

//   return (
//     <>
//       {/* ✅ NAVBAR */}
//       <Navbar />

//       <div className="container py-4">
//         <h2 className="mb-4 fw-bold">🗣 Student Feedback</h2>

//         {feedbacks.length === 0 ? (
//           <p>No feedback available for your courses.</p>
//         ) : (
//           <div className="row g-3">
//             {feedbacks.map((f) => (
//               <div className="col-md-6" key={f.feedbackId}>
//                 <div
//                   className={`card h-100 shadow-sm ${
//                     f.isReported ? "border-danger" : ""
//                   }`}
//                 >
//                   <div className="card-body">
//                     <h5 className="card-title">{f.courseTitle}</h5>

//                     <p className="text-warning mb-1">
//                       {"★".repeat(f.rating)}
//                       {"☆".repeat(5 - f.rating)}
//                     </p>

//                     <p className="fst-italic">"{f.comment}"</p>

//                     <p className="text-muted small mb-2">
//                       By {f.studentName} •{" "}
//                       {new Date(f.createdAt).toLocaleDateString()}
//                     </p>

//                     {f.isReported ? (
//                       <span className="badge bg-danger">
//                         🚩 Reported
//                       </span>
//                     ) : (
//                       <button
//                         className="btn btn-sm btn-outline-danger"
//                         onClick={() => reportFeedback(f.feedbackId)}
//                       >
//                         🚩 Report Feedback
//                       </button>
//                     )}
//                   </div>
//                 </div>
//               </div>
//             ))}
//           </div>
//         )}
//       </div>
//     </>
//   );
// }





import { useEffect, useState } from "react";
import api from "../../services/api";
import Navbar from "../../components/NavbarPublic.jsx";

export default function MentorFeedback() {
  const [feedbacks, setFeedbacks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const fetchFeedbacks = async () => {
    try {
      const res = await api.get("/feedback/mentor");
      setFeedbacks(res.data);
    } catch (err) {
      console.error(err);
      setError("Failed to load feedback");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchFeedbacks();
  }, []);

  const reportFeedback = async (feedbackId) => {
    if (!window.confirm("Are you sure you want to report this feedback?"))
      return;

    try {
      const res = await api.post(`/feedback/${feedbackId}/report`);

      // Update feedback in state
      setFeedbacks((prev) =>
        prev.map((f) =>
          f.feedbackId === feedbackId
            ? { ...f, isReported: res.data.isReported }
            : f
        )
      );

      alert(res.data.message);
    } catch (err) {
      console.error(err);
      alert(err.response?.data?.message || "Failed to report feedback");
    }
  };

  if (loading) return <p className="p-4">Loading feedback...</p>;
  if (error) return <p className="p-4 text-danger">{error}</p>;

  return (
    <>
      <Navbar />
      <div className="container py-4">
        <h2 className="mb-4 fw-bold">🗣 Student Feedback</h2>

        {feedbacks.length === 0 ? (
          <p>No feedback available for your courses.</p>
        ) : (
          <div className="row g-3">
            {feedbacks.map((f) => (
              <div className="col-md-6" key={f.feedbackId}>
                <div
                  className={`card h-100 shadow-sm ${
                    f.isReported ? "border-danger" : ""
                  }`}
                >
                  <div className="card-body">
                    <h5 className="card-title">{f.courseTitle}</h5>

                    <p className="text-warning mb-1">
                      {"★".repeat(f.rating)}
                      {"☆".repeat(5 - f.rating)}
                    </p>

                    <p className="fst-italic">"{f.comment}"</p>

                    <p className="text-muted small mb-2">
                      By {f.studentName} •{" "}
                      {new Date(f.createdAt).toLocaleDateString()}
                    </p>

                    {f.isReported ? (
                      <span className="badge bg-danger">🚩 Reported</span>
                    ) : (
                      <button
                        className="btn btn-sm btn-outline-danger"
                        onClick={() => reportFeedback(f.feedbackId)}
                      >
                        🚩 Report Feedback
                      </button>
                    )}
                  </div>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </>
  );
}
