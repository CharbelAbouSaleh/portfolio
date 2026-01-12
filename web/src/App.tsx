import { useEffect, useState } from "react";

type Profile = {
  name: string;
  title: string;
  location: string;
};

type Project = {
  id: number;
  title: string;
  description: string | null;
  techStack: string | null;
  repoUrl: string | null;
  liveUrl: string | null;
  startDate: string | null; // DateOnly comes as "YYYY-MM-DD" in JSON
};

const API_BASE = import.meta.env.VITE_API_BASE_URL;

export default function App() {
  const [profile, setProfile] = useState<Profile | null>(null);
  const [projects, setProjects] = useState<Project[]>([]);
  const [error, setError] = useState<string>("");

  useEffect(() => {
    // Load profile
    fetch(`${API_BASE}/api/profile`)
      .then((r) => {
        if (!r.ok) throw new Error(`Profile HTTP ${r.status}`);
        return r.json();
      })
      .then(setProfile)
      .catch((e) => setError(String(e)));

    // Load projects
    fetch(`${API_BASE}/api/projects`)
      .then((r) => {
        if (!r.ok) throw new Error(`Projects HTTP ${r.status}`);
        return r.json();
      })
      .then(setProjects)
      .catch((e) => setError(String(e)));
  }, []);

  return (
    <div style={{ fontFamily: "system-ui", padding: 24, maxWidth: 900 }}>
      <h1 style={{ marginBottom: 8 }}>Portfolio</h1>

      {error && <p style={{ color: "red" }}>Error: {error}</p>}

      {!profile && !error && <p>Loading...</p>}

      {profile && (
        <section style={{ marginTop: 16, marginBottom: 28 }}>
          <h2 style={{ margin: 0 }}>{profile.name}</h2>
          <p style={{ margin: "8px 0 0 0" }}>{profile.title}</p>
          <p style={{ margin: "8px 0 0 0" }}>{profile.location}</p>
        </section>
      )}

      <section>
        <h2 style={{ marginBottom: 12 }}>Projects</h2>

        {projects.length === 0 && !error && (
          <p>No projects yet. Add some rows in Supabase.</p>
        )}

        <div style={{ display: "grid", gap: 12 }}>
          {projects.map((p) => (
            <div
              key={p.id}
              style={{
                border: "1px solid #ddd",
                borderRadius: 10,
                padding: 14,
              }}
            >
              <div style={{ display: "flex", justifyContent: "space-between", gap: 12 }}>
                <h3 style={{ margin: 0 }}>{p.title}</h3>
                {p.startDate && (
                  <span style={{ opacity: 0.7, whiteSpace: "nowrap" }}>{p.startDate}</span>
                )}
              </div>

              {p.description && <p style={{ margin: "10px 0 0 0" }}>{p.description}</p>}

              {p.techStack && (
                <p style={{ margin: "10px 0 0 0", opacity: 0.85 }}>
                  <strong>Tech:</strong> {p.techStack}
                </p>
              )}

              <div style={{ marginTop: 10, display: "flex", gap: 12, flexWrap: "wrap" }}>
                {p.repoUrl && (
                  <a href={p.repoUrl} target="_blank" rel="noreferrer">
                    Repo
                  </a>
                )}
                {p.liveUrl && (
                  <a href={p.liveUrl} target="_blank" rel="noreferrer">
                    Live
                  </a>
                )}
              </div>
            </div>
          ))}
        </div>
      </section>
    </div>
  );
}
