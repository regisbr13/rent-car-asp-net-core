// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    $(".createRole").click(function () {
        $("#modal").load("/Niveis-de-acesso/Novo", function () {
            $("#modal").modal();
        })
    })
});

$(function () {
    $(".editRole").click(function () {
        var id = $(this).attr("data-id");
        $("#modal").load("/Niveis-de-acesso/Editar?Id=" + id, function () {
            $("#modal").modal();
        })
    })
});

$(function () {
    $(".delRole").click(function () {
        var id = $(this).attr("data-id");
        $("#modal").load("/Niveis-de-acesso/Excluir?Id=" + id, function () {
            $("#modal").modal();
        })
    })
});

$(function () {
    $(".editUser").click(function () {
        var id = $(this).attr("data-id");
        $("#modal").load("/Atualizar?Id=" + id, function () {
            $("#modal").modal();
        })
    })
});

$(function () {
    $(".addressCreate").click(function () {
        var userId = $(this).attr("data-userId");
        $("#modal").load("/Enderecos/Novo?UserId=" + userId, function () {
            $("#modal").modal();
        })
    })
});

$(function () {
    $(".editAddress").click(function () {
        var id = $(this).attr("data-id");
        $("#modal").load("/Enderecos/Editar?Id=" + id, function () {
            $("#modal").modal();
        })
    })
});

$(function () {
    $(".delAddress").click(function () {
        var id = $(this).attr("data-id");
        $("#modal").load("/Enderecos/Deletar?Id=" + id, function () {
            $("#modal").modal();
        })
    })
});