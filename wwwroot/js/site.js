// Timecode "REC" en haut de page (25 images/s depuis le chargement)
(() => {
  const el = document.getElementById("tc");
  if (!el) return;
  const t0 = performance.now();
  const pad = n => String(n).padStart(2, "0");
  let last = -1;
  const tick = () => {
    const fr = Math.floor((performance.now() - t0) / 40);
    if (fr !== last) {
      last = fr;
      el.textContent = `${pad(Math.floor(fr / 90000))}:${pad(Math.floor(fr / 1500) % 60)}:${pad(Math.floor(fr / 25) % 60)}:${pad(fr % 25)}`;
    }
    requestAnimationFrame(tick);
  };
  tick();
})();

// Toggle Grille / Liste, mémorisé dans le navigateur
(() => {
  const films = document.querySelector(".films");
  if (!films) return;
  const buttons = films.querySelectorAll("[data-set-view]");
  const apply = view => {
    films.dataset.view = view;
    buttons.forEach(b => b.setAttribute("aria-pressed", String(b.dataset.setView === view)));
  };
  try {
    const saved = localStorage.getItem("films-view");
    if (saved === "grid" || saved === "list") apply(saved);
  } catch { }
  buttons.forEach(b => b.addEventListener("click", () => {
    apply(b.dataset.setView);
    try { localStorage.setItem("films-view", b.dataset.setView); } catch { }
  }));
})();

// Lecteur plein écran : un seul iframe chargé à la demande
(() => {
  const dialog = document.getElementById("player");
  if (!dialog) return;
  const iframe = document.getElementById("player-iframe");
  const bars = document.getElementById("player-bars");
  const title = document.getElementById("player-title");
  const link = document.getElementById("player-link");

  document.addEventListener("click", e => {
    const clip = e.target.closest("[data-video]");
    if (!clip) return;
    const id = clip.dataset.video;
    iframe.src = `https://www.youtube.com/embed/${encodeURIComponent(id)}?autoplay=1&rel=0`;
    title.textContent = clip.dataset.title;
    link.href = `https://www.youtube.com/watch?v=${encodeURIComponent(id)}`;
    bars.classList.remove("letterbox");
    void bars.offsetWidth; // relance l'animation des bandes
    bars.classList.add("letterbox");
    dialog.showModal();
  });

  document.getElementById("player-close").addEventListener("click", () => dialog.close());
  dialog.addEventListener("click", e => { if (e.target === dialog) dialog.close(); });
  dialog.addEventListener("close", () => { iframe.src = "about:blank"; });
})();
