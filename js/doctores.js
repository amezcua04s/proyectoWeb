const doctores = [
    { img: "img/juan.jpg", title: 'Dr. Juan Pérez', desc: "Anestesiólogo", disponible:"img/disp.jpeg"},
    { img: "img/desiree.jpeg", title: 'Dra. Desiree García', desc: "Cardióloga", disponible:"img/no disp.jpeg"},
    { img: "img/samuel.jpeg", title: 'Dr. Samuel', desc: "Pediatra", disponible:"img/no disp.jpeg"},
    { img: "img/paola.jpeg", title: 'Dra. Paola Vives', desc: "Cirujana", disponible:"img/disp.jpeg"},
    { img: "img/ester.jpeg", title: 'Dra. Ester Segura', desc: "Neuróloga", disponible:"img/disp.jpeg"},
    { img: "img/maria.jpeg", title: 'Dra. María Consuelo', desc: "Ortopédica", disponible:"img/no disp.jpeg"},
    { img: "img/pascual.jpeg", title: 'Dr. Pascual Matas', desc: "Cardiólogo", disponible:"img/no disp.jpeg"},
    { img: "img/jorge.jpeg", title: 'Dr. Jorge Palacios', desc: "Urológo", disponible:"img/disp.jpeg"},
    { img: "img/jesus.jpeg", title: 'Dr. Jesus Paz', desc: "Psquiatra", disponible:"img/disp.jpeg"}
  ];
  
  const container = document.getElementById("doctor-list");
  
  doctores.forEach(d => {
    const col = document.createElement("div");
    col.className = "col-md-4 col-sm-6 mb-4";
    col.innerHTML = `
      <div class="card">
        <img src="${d.img}" class="card-img-top" alt="${d.title}">
        <div class="card-body">
          <h5 class="card-title">${d.title}</h5>
          <p>${d.desc}</p>
          <img src="${d.disponible}" class="position-absolute bottom-0 end-0 peque"></img>
        </div>
      </div>
    `;
    container.appendChild(col);
  });
  