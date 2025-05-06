const doctores = [
    { img: "img/admin/juan.jpg", title: 'Dr. Juan Pérez', desc: "Anestesiólogo"},
    { img: "img/admin/desiree.jpeg", title: 'Dra. Desiree García', desc: "Cardióloga",},
    { img: "img/admin/samuel.jpeg", title: 'Dr. Samuel', desc: "Pediatra", },
    { img: "img/admin/paola.jpeg", title: 'Dra. Paola Vives', desc: "Cirujana",},
    { img: "img/admin/ester.jpeg", title: 'Dra. Ester Segura', desc: "Neuróloga", },
    { img: "img/admin/maria.jpeg", title: 'Dra. María Consuelo', desc: "Ortopédica", },
    { img: "img/admin/pascual.jpeg", title: 'Dr. Pascual Matas', desc: "Cardiólogo", },
    { img: "img/admin/jorge.jpeg", title: 'Dr. Jorge Palacios', desc: "Urológo", },
    { img: "img/admin/jesus.jpeg", title: 'Dr. Jesus Paz', desc: "Psquiatra", }
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
          <button class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#exampleModal">Ver más</button>
        </div>
      </div>
    `;
    container.appendChild(col);
  });
  