import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import api from "../../services/api";
import Loading from "../../components/Loading";

export default function AdminFeedback() {
  const [tab, setTab] = useState("reported"); // reported | all
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);

  const load = async () => {
    setLoading(true);
    const url =
      tab === "reported" ? "/admin/feedback/reported" : "/admin/feedback/all";
    const res = await api.get(url);
    setItems(res.data);
    setLoading(false);
  };

  useEffect(() => {
    load();
    // eslint-disable-next-line
  }, [tab]);

  const resolveReport = async (id) => {
    if (!confirm("Resolve this report (unreport)?")) return;
    await api.put(`/admin/feedback/${id}/resolve`);
    load();
  };

  const deleteFeedback = async (id) => {
    if (!confirm("Soft delete this feedback?")) return;
    await api.put(`/admin/feedback/${id}/delete`);
    load();
  };

  const restoreFeedback = async (id) => {
    if (!confirm("Restore this feedback?")) return;
    await api.put(`/admin/feedback/${id}/restore`);
    load();
  };

  const blockStudent = async (studentId) => {
    if (!confirm("Block this student? (They cannot login again)")) return;
    await api.put(`/admin/users/${studentId}/block`);
    load();
  };

  const unblockStudent = async (studentId) => {
    if (!confirm("Unblock this student?")) return;
    await api.put(`/admin/users/${studentId}/unblock`);
    load();
  };

  return (
    <div>
      <nav className="navbar navbar-dark bg-dark px-3">
        <Link className="navbar-brand fw-bold" to="/admin">
          Admin
        </Link>
        <div className="ms-auto d-flex gap-2">
          <Link className="btn btn-outline-light" to="/admin/categories">
            Category
          </Link>
          <Link className="btn btn-outline-light" to="/admin/users">
            Users
          </Link>
          <Link className="btn btn-outline-light" to="/admin/feedback">
            Feedback
          </Link>
          <Link className="btn btn-outline-light" to="/admin/profile">
            Profile
          </Link>
        </div>
      </nav>

      <div className="container py-4">
        <div className="d-flex justify-content-between align-items-center mb-3">
          <h4 className="fw-bold mb-0">Feedback Management</h4>

          <div className="btn-group">
            <button
              className={`btn btn-sm ${
                tab === "reported" ? "btn-danger" : "btn-outline-danger"
              }`}
              onClick={() => setTab("reported")}
            >
              Reported
            </button>
            <button
              className={`btn btn-sm ${
                tab === "all" ? "btn-dark" : "btn-outline-dark"
              }`}
              onClick={() => setTab("all")}
            >
              All
            </button>
          </div>
        </div>

        {loading ? (
          <Loading />
        ) : (
          <div className="card shadow-sm p-3">
            <div className="table-responsive">
              <table className="table table-bordered table-hover align-middle">
                <thead className="table-dark">
                  <tr>
                    <th style={{ width: 70 }}>ID</th>
                    <th>Course</th>
                    <th>Student</th>
                    <th style={{ width: 90 }}>Rating</th>
                    <th>Comment</th>
                    <th style={{ width: 140 }}>Status</th>
                    <th style={{ width: 300 }}>Actions</th>
                  </tr>
                </thead>

                <tbody>
                  {items.map((f) => (
                    <tr key={f.feedbackId}>
                      <td>{f.feedbackId}</td>
                      <td>{f.courseTitle}</td>
                      <td>
                        <div className="fw-semibold">{f.studentName}</div>
                        <div className="text-muted small">@{f.studentUsername}</div>
                        {f.studentIsBlocked ? (
                          <span className="badge bg-danger mt-1">Blocked</span>
                        ) : (
                          <span className="badge bg-success mt-1">Active</span>
                        )}
                      </td>
                      <td className="text-warning">
                        {"★".repeat(f.rating)}
                        {"☆".repeat(5 - f.rating)}
                      </td>
                      <td style={{ maxWidth: 420 }}>{f.comment}</td>
                      <td>
                        {f.isDeleted ? (
                          <span className="badge bg-secondary">Deleted</span>
                        ) : f.isReported ? (
                          <span className="badge bg-danger">Reported</span>
                        ) : (
                          <span className="badge bg-success">OK</span>
                        )}
                      </td>
                      <td>
                        <div className="d-flex flex-wrap gap-2">
                          {f.isReported && !f.isDeleted && (
                            <button
                              className="btn btn-sm btn-outline-success"
                              onClick={() => resolveReport(f.feedbackId)}
                            >
                              Resolve
                            </button>
                          )}

                          {!f.isDeleted ? (
                            <button
                              className="btn btn-sm btn-outline-danger"
                              onClick={() => deleteFeedback(f.feedbackId)}
                            >
                              Delete Feedback
                            </button>
                          ) : (
                            <button
                              className="btn btn-sm btn-outline-primary"
                              onClick={() => restoreFeedback(f.feedbackId)}
                            >
                              Restore
                            </button>
                          )}

                          {!f.studentIsBlocked ? (
                            <button
                              className="btn btn-sm btn-danger"
                              onClick={() => blockStudent(f.studentId)}
                            >
                              Block Student
                            </button>
                          ) : (
                            <button
                              className="btn btn-sm btn-success"
                              onClick={() => unblockStudent(f.studentId)}
                            >
                              Unblock Student
                            </button>
                          )}
                        </div>
                      </td>
                    </tr>
                  ))}

                  {items.length === 0 && (
                    <tr>
                      <td colSpan="7" className="text-center text-muted">
                        No feedback found.
                      </td>
                    </tr>
                  )}
                </tbody>
              </table>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
