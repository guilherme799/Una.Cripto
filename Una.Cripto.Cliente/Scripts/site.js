$(function () {
    $('#spnVetorInicializacao').hide();
    $('#btnCriptografar').click(submitForm);
    $('#btnDescriptografar').click(submitForm);
    $('[name=Mode]').click(radioCBC_Checked);
});

var submitForm = function () {
    $('#Metodo').val($(this).val());
};

var radioCBC_Checked = function (e) {
    $('#spnVetorInicializacao').toggle('fast');
};