import { useEffect, useState } from "react";

type Profile = {
  name: string;
  title: string;
  location: string;
};



export default function App() {
  const [profile, setProfile] = useState<Profile | null>(null);
  const [error, setError] = useState<string>("");

  useEffect(() => {
    const API_BASE = import.meta.env.VITE_API_BASE_URL;
    fetch(`${API_BASE}/api/profile`)
      .then((r) => {
        if (!r.ok) throw new Error(`HTTP ${r.status}`);
        return r.json();
      })
      .then(setProfile)
      .catch((e) => setError(String(e)));
  }, []);

  return (
    <div style={{ fontFamily: "system-ui", padding: 24 }}>
      <h1>Portfolio</h1>

      {error && <p style={{ color: "red" }}>Error: {error}</p>}

      {!profile && !error && <p>Loading...</p>}

      {profile && (
        <div>
          <h2>{profile.name}</h2>
          <p>{profile.title}</p>
          <p>{profile.location}</p>
        </div>
      )}
    </div>
  );
}
