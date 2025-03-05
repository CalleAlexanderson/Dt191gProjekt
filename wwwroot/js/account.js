// Globala konstanter och variabler

// --------------------------------------------------
// Initiera globala variabler och händelsehanterare
function init() {
    let accSaveBtn = document.getElementById('account-save-btn');
    accSaveBtn.addEventListener("click", ()=>{
        document.getElementById('status-msg-div').style.display = "none";
    })
} // Slut init
window.addEventListener('load', init);
// --------------------------------------------------