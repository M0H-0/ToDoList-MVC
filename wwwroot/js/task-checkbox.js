document.addEventListener('DOMContentLoaded',function(){
    const checkboxes=document.querySelectorAll(".check-box");
    checkboxes.forEach(checkbox =>checkbox.addEventListener('change',function(){
        const Id = this.dataset.id;
        const IsDone= this.checked;
        const token = document.querySelector('input[name="__RequestVerificationToken"]').value;
        fetch('/Tasks/SetDoneStatus',{
            method :'POST',
            headers:{
            'Content-Type':'application/json',
            'RequestVerificationToken': token
            },
            body: JSON.stringify({
                "Id": Id,
                "IsDone" : IsDone
            })
        });
    }));
});