// import React, { useEffect, useState } from "react";
// import { useParams } from "react-router-dom";
// import api from "../../services/api";
// import "./StudentFeedback.css";



// export default function StudentFeedback() {
//   const { courseId } = useParams();

//   const [rating, setRating] = useState(5);
//   const [comment, setComment] = useState("");
//   const [feedbacks, setFeedbacks] = useState([]);
//   const [loading, setLoading] = useState(false);
//   const [msg, setMsg] = useState("");

//   const fetchFeedback = async () => {
//     try {
//       const res = await api.get(`/feedback/course/${courseId}`);
//       setFeedbacks(res.data);
//     } catch (err) {
//       console.error(err);
//     }
//   };

//   useEffect(() => {
//     fetchFeedback();
//   }, [courseId]);

//   const submitFeedback = async (e) => {
//     e.preventDefault();
//     setLoading(true);
//     setMsg("");

//     try {
//       await api.post("/feedback", {
//         courseId,
//         rating,
//         comment,
//       });

//       setMsg("✅ Feedback submitted successfully");
//       setRating(5);
//       setComment("");
//       fetchFeedback();
//     } catch (err) {
//       setMsg(err.response?.data?.message || "❌ Feedback already submitted");
//     } finally {
//       setLoading(false);
//     }
//   };

//   return (
//     <div className="feedback-wrapper">
//       <div className="feedback-card-main">
//         <h2>⭐ Course Feedback</h2>

//         <form onSubmit={submitFeedback} className="feedback-form">
//           <label className="fw-semibold">Your Rating</label>

//           <div className="star-rating">
//             {[1, 2, 3, 4, 5].map((s) => (
//               <span
//                 key={s}
//                 className={s <= rating ? "star active" : "star"}
//                 onClick={() => setRating(s)}
//               >
//                 ★
//               </span>
//             ))}
//           </div>

//           <label className="fw-semibold">Your Comment</label>
//           <textarea
//             required
//             placeholder="Share your experience with this course..."
//             value={comment}
//             onChange={(e) => setComment(e.target.value)}
//           />

//           <button disabled={loading}>
//             {loading ? "Submitting..." : "Submit Feedback"}
//           </button>

//           {msg && <p className="message">{msg}</p>}
//         </form>
//       </div>

//       <div className="feedback-list">
//         <h3>🗣 Student Reviews</h3>

//         {feedbacks.length === 0 && (
//           <p className="text-muted">No feedback yet</p>
//         )}

//         {feedbacks.map((f) => (
//           <div className="feedback-card" key={f.feedbackId}>
//             <strong>{f.studentName}</strong>
//             <div className="stars">
//               {"★".repeat(f.rating)}
//               {"☆".repeat(5 - f.rating)}
//             </div>
//             <p>{f.comment}</p>
//             <small>
//               {new Date(f.createdAt).toLocaleDateString()}
//             </small>
//           </div>
//         ))}
//       </div>
//     </div>
//   );
// }

// import React, { useEffect, useState } from "react";
// import { useParams } from "react-router-dom";
// import api from "../../services/api";
// import Navbar from "../../components/NavbarPublic.jsx"; // ✅ NAVBAR
// import "./StudentFeedback.css";

// export default function StudentFeedback() {
//   const { courseId } = useParams();

//   const [rating, setRating] = useState(5);
//   const [comment, setComment] = useState("");
//   const [feedbacks, setFeedbacks] = useState([]);
//   const [loading, setLoading] = useState(false);
//   const [msg, setMsg] = useState("");

//   // Edit states
//   const [isEditing, setIsEditing] = useState(false);
//   const [editFeedbackId, setEditFeedbackId] = useState(null);

//   const fetchFeedback = async () => {
//     try {
//       const res = await api.get(`/feedback/course/${courseId}`);
//       setFeedbacks(res.data);
//     } catch (err) {
//       console.error(err);
//     }
//   };

//   useEffect(() => {
//     fetchFeedback();
//   }, [courseId]);

//   const submitFeedback = async (e) => {
//     e.preventDefault();
//     setLoading(true);
//     setMsg("");

//     try {
//       if (isEditing && editFeedbackId) {
//         // ✏️ UPDATE
//         await api.put(`/feedback/${editFeedbackId}`, {
//           rating,
//           comment,
//         });
//         setMsg("✏️ Feedback updated successfully");
//       } else {
//         // ➕ CREATE
//         await api.post("/feedback", {
//           courseId,
//           rating,
//           comment,
//         });
//         setMsg("✅ Feedback submitted successfully");
//       }

//       setRating(5);
//       setComment("");
//       setIsEditing(false);
//       setEditFeedbackId(null);
//       fetchFeedback();
//     } catch (err) {
//       setMsg(err.response?.data?.message || "❌ Something went wrong");
//     } finally {
//       setLoading(false);
//     }
//   };

//   const handleEdit = (f) => {
//     setIsEditing(true);
//     setEditFeedbackId(f.feedbackId);
//     setRating(f.rating);
//     setComment(f.comment);
//     window.scrollTo({ top: 0, behavior: "smooth" });
//   };

//   return (
//     <>
//       {/* ✅ NAVBAR */}
//       <Navbar />

//       <div className="feedback-wrapper">
//         <div className="feedback-card-main">
//           <h2>⭐ Course Feedback</h2>

//           <form onSubmit={submitFeedback} className="feedback-form">
//             <label className="fw-semibold">Your Rating</label>

//             <div className="star-rating">
//               {[1, 2, 3, 4, 5].map((s) => (
//                 <span
//                   key={s}
//                   className={s <= rating ? "star active" : "star"}
//                   onClick={() => setRating(s)}
//                 >
//                   ★
//                 </span>
//               ))}
//             </div>

//             <label className="fw-semibold">Your Comment</label>
//             <textarea
//               required
//               placeholder="Share your experience with this course..."
//               value={comment}
//               onChange={(e) => setComment(e.target.value)}
//             />

//             <button disabled={loading}>
//               {loading
//                 ? isEditing
//                   ? "Updating..."
//                   : "Submitting..."
//                 : isEditing
//                 ? "Update Feedback"
//                 : "Submit Feedback"}
//             </button>

//             {isEditing && (
//               <button
//                 type="button"
//                 className="cancel-btn"
//                 onClick={() => {
//                   setIsEditing(false);
//                   setEditFeedbackId(null);
//                   setRating(5);
//                   setComment("");
//                 }}
//               >
//                 Cancel Edit
//               </button>
//             )}

//             {msg && <p className="message">{msg}</p>}
//           </form>
//         </div>

//         <div className="feedback-list">
//           <h3>🗣 Student Reviews</h3>

//           {feedbacks.length === 0 && (
//             <p className="text-muted">No feedback yet</p>
//           )}

//           {feedbacks.map((f) => (
//             <div className="feedback-card" key={f.feedbackId}>
//               <strong>{f.studentName}</strong>

//               <div className="stars">
//                 {"★".repeat(f.rating)}
//                 {"☆".repeat(5 - f.rating)}
//               </div>

//               <p>{f.comment}</p>

//               <small>
//                 {new Date(f.createdAt).toLocaleDateString()}
//               </small>

//               {/* ✏️ EDIT (only own feedback) */}
//               {f.isOwnFeedback && (
//                 <button
//                   className="edit-btn"
//                   onClick={() => handleEdit(f)}
//                 >
//                   ✏️ Edit
//                 </button>
//               )}
//             </div>
//           ))}
//         </div>
//       </div>
//     </>
//   );
// }

import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import api from "../../services/api";
import Navbar from "../../components/NavbarPublic.jsx"; 
import "./StudentFeedback.css";

export default function StudentFeedback() {
  const { courseId } = useParams();

  const [rating, setRating] = useState(5);
  const [comment, setComment] = useState("");
  const [feedbacks, setFeedbacks] = useState([]);
  const [loading, setLoading] = useState(false);
  const [msg, setMsg] = useState("");

  // Edit states
  const [isEditing, setIsEditing] = useState(false);
  const [editFeedbackId, setEditFeedbackId] = useState(null);

  // Fetch feedbacks
  const fetchFeedback = async () => {
    try {
      const res = await api.get(`/feedback/course/${courseId}`);
      setFeedbacks(res.data);
    } catch (err) {
      console.error(err);
    }
  };

  useEffect(() => {
    fetchFeedback();
  }, [courseId]);

  // Submit new or edited feedback
  const submitFeedback = async (e) => {
    e.preventDefault();
    setLoading(true);
    setMsg("");

    try {
      if (isEditing && editFeedbackId) {
        // Update feedback
        await api.put(`/feedback/${editFeedbackId}`, {
          rating,
          comment,
        });
        setMsg("✏️ Feedback updated successfully");
      } else {
        // Create new feedback
        await api.post("/feedback", {
          courseId,
          rating,
          comment,
        });
        setMsg("✅ Feedback submitted successfully");
      }

      // Reset form
      setRating(5);
      setComment("");
      setIsEditing(false);
      setEditFeedbackId(null);

      // Refresh feedbacks
      fetchFeedback();
    } catch (err) {
      setMsg(err.response?.data?.message || "❌ Something went wrong");
    } finally {
      setLoading(false);
    }
  };

  // Start editing feedback
  const handleEdit = (f) => {
    setIsEditing(true);
    setEditFeedbackId(f.feedbackId);
    setRating(f.rating);
    setComment(f.comment);
    window.scrollTo({ top: 0, behavior: "smooth" });
  };

  // Cancel editing
  const cancelEdit = () => {
    setIsEditing(false);
    setEditFeedbackId(null);
    setRating(5);
    setComment("");
    setMsg("");
  };

  return (
    <>
      <Navbar />

      <div className="feedback-wrapper">
        {/* Feedback Form */}
        <div className="feedback-card-main">
          <h2>⭐ Course Feedback</h2>

          <form onSubmit={submitFeedback} className="feedback-form">
            <label className="fw-semibold">Your Rating</label>
            <div className="star-rating">
              {[1, 2, 3, 4, 5].map((s) => (
                <span
                  key={s}
                  className={s <= rating ? "star active" : "star"}
                  onClick={() => setRating(s)}
                >
                  ★
                </span>
              ))}
            </div>

            <label className="fw-semibold">Your Comment</label>
            <textarea
              required
              placeholder="Share your experience with this course..."
              value={comment}
              onChange={(e) => setComment(e.target.value)}
            />

            <div className="d-flex gap-2">
              <button type="submit" disabled={loading} className="btn btn-primary">
                {loading
                  ? isEditing
                    ? "Updating..."
                    : "Submitting..."
                  : isEditing
                  ? "Update Feedback"
                  : "Submit Feedback"}
              </button>

              {isEditing && (
                <button
                  type="button"
                  className="btn btn-secondary"
                  onClick={cancelEdit}
                >
                  Cancel
                </button>
              )}
            </div>

            {msg && <p className="message">{msg}</p>}
          </form>
        </div>

        {/* Feedback List */}
        <div className="feedback-list">
          <h3>🗣 Student Reviews</h3>

          {feedbacks.length === 0 ? (
            <p className="text-muted">No feedback yet</p>
          ) : (
            feedbacks.map((f) => (
              <div className="feedback-card" key={f.feedbackId}>
                <strong>{f.studentName}</strong>
                <div className="stars">
                  {"★".repeat(f.rating)}
                  {"☆".repeat(5 - f.rating)}
                </div>
                <p>{f.comment}</p>
                <small>{new Date(f.createdAt).toLocaleDateString()}</small>

                {/* Edit button only for own feedback */}
                {f.isOwnFeedback && (
                  <button
                    className="btn btn-sm btn-outline-warning mt-1"
                    onClick={() => handleEdit(f)}
                  >
                    ✏️ Edit
                  </button>
                )}
              </div>
            ))
          )}
        </div>
      </div>
    </>
  );
}

