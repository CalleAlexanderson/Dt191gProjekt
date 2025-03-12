// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


// Globala konstanter och variabler

// --------------------------------------------------
// Initiera globala variabler och händelsehanterare
function headerInit() {
    let navbtn = document.getElementById('nav-toggle-btn');
    let navDiv = document.getElementById('nav-div');
    let dropDownBtn = document.getElementById('profile-button');
    let dropDown = document.getElementById('profile-dropdown');
    let spans = document.getElementsByClassName('nav-menu-span');

    navbtn.addEventListener("click", () => {
        if (navDiv.classList.contains("nav-hidden")) {
            navDiv.classList.remove("nav-hidden")
            for (let index = 0; index < spans.length; index++) {
                spans[index].classList.add("open")
            }
        } else {
            navDiv.classList.add("nav-hidden")
            for (let index = 0; index < spans.length; index++) {
                spans[index].classList.remove("open")
            }
        }
    })

    if (dropDownBtn != null) {
        dropDownBtn.addEventListener("mouseenter", () => {
            console.log("funkar");
            setTimeout(() => {
                dropDown.classList.remove("hidden");
            }, 250);
        })
        dropDownBtn.addEventListener("mouseleave", () => {
            dropDown.classList.add("hidden");
        })

        dropDown.addEventListener("mouseenter", () => {
            dropDown.classList.remove("hidden");
        })

        dropDown.addEventListener("mouseleave", () => {
            dropDown.classList.add("hidden");
        })
    }

} // Slut init
window.addEventListener('load', headerInit);
// --------------------------------------------------
