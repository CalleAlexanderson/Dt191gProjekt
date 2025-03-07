// Globala konstanter och variabler

// --------------------------------------------------
// Initiera globala variabler och händelsehanterare
function accountInit() {
    let accSaveBtn = document.getElementById('account-save-btn');
    
    if (accSaveBtn != null) {
        accSaveBtn.addEventListener("click", ()=>{
            document.getElementById('status-msg-div').style.display = "none";
        })
    }

} // Slut init
window.addEventListener('load', accountInit);
// --------------------------------------------------