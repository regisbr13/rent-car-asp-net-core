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

$(function () {
    $(".delUser").click(function () {
        var id = $(this).attr("data-id");
        $("#modal").load("/Excluir?Id=" + id, function () {
            $("#modal").modal();
        })
    })
});

$(function () {
    $(".editUserRole").click(function () {
        var id = $(this).attr("data-id");
        $("#modal").load("/Editar-nivel-de-acesso?Id=" + id, function () {
            $("#modal").modal();
        })
    })
});

$(function () {
    $(".createBalace").click(function () {
        $("#modal").load("/Saldos/Novo", function () {
            $("#modal").modal();
        })
    })
});

$(function () {
    $(".editBalance").click(function () {
        var id = $(this).attr("data-id");
        $("#modal").load("/Saldos/Editar?Id=" + id, function () {
            $("#modal").modal();
        })
    })
});

$(function () {
    $(".delBalance").click(function () {
        var id = $(this).attr("data-id");
        $("#modal").load("/Saldos/Deletar?Id=" + id, function () {
            $("#modal").modal();
        })
    })
});

$(function () {
    $(".createCar").click(function () {
        $("#modal").load("/Carros/Novo", function () {
            $("#modal").modal();
        })
    })
});

$(function () {
    $(".delCar").click(function () {
        var id = $(this).attr("data-id");
        $("#modal").load("/Carros/Deletar?Id=" + id, function () {
            $("#modal").modal();
        })
    })
});

$(function () {
    $(".editCar").click(function () {
        var id = $(this).attr("data-id");
        $("#modal").load("/Carros/Editar?Id=" + id, function () {
            $("#modal").modal();
        })
    })
});

function LoadImg(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        $(".img").show();
        reader.onload = function (e) {
            $(".img").attr('src', e.target.result).width(200).height(200);
        }
    }
    reader.readAsDataURL(input.files[0]);
}



// Máscaras:
$(function () {
    $(".cpf").mask("999.999.999-99");
});
$(function () {
    $(".phone").mask("(99) 9 9999-9999");
});

    function k(i) {
        var v = i.value.replace(/\D/g, '');
        v = (v / 100).toFixed(2) + '';
        v = v.replace(".", ",");
        v = v.replace(/(\d)(\d{3})(\d{3}),/g, "$1.$2.$3,");
        v = v.replace(/(\d)(\d{3}),/g, "$1.$2,");
        i.value = v;
    }