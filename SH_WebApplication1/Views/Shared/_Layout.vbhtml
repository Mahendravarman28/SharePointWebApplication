<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>@ViewBag.Title — SharePoint Platform</title>
    @Styles.Render("~/Content/css")
    @Scripts.Render("~/bundles/modernizr")
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css">
    <style>
        body { background-color: #f8f9fa; }
        .navbar-brand { font-weight: 700; letter-spacing: 0.5px; }
        .nav-item.dropdown:hover .dropdown-menu { display: block; }
        .dropdown-menu { margin-top: 0; }
    </style>
</head>
<body>
    <nav class="navbar navbar-expand-lg navbar-dark bg-dark shadow-sm">
        <div class="container-fluid">
            <a class="navbar-brand" href="@Url.Action("Index", "Home")">
                <i class="bi bi-grid-3x3-gap-fill me-1"></i> SP Platform
            </a>
            <button type="button" class="navbar-toggler" data-bs-toggle="collapse" data-bs-target="#navbarMain"
                    aria-controls="navbarMain" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarMain">
                <ul class="navbar-nav me-auto mb-2 mb-lg-0">

                    <li class="nav-item">
                        @Html.ActionLink("Home", "Index", "Home", New With {.area = ""}, New With {.class = "nav-link"})
                    </li>

                    <!-- Module A -->
                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">
                            <i class="bi bi-table me-1"></i> Lists
                        </a>
                        <ul class="dropdown-menu dropdown-menu-dark">
                            <li>@Html.ActionLink("All Lists", "Index", "ListBuilder", Nothing, New With {.class = "dropdown-item"})</li>
                            <li>@Html.ActionLink("Create New List", "Create", "ListBuilder", Nothing, New With {.class = "dropdown-item"})</li>
                        </ul>
                    </li>

                    <!-- Module B -->
                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">
                            <i class="bi bi-ui-checks-grid me-1"></i> Forms
                        </a>
                        <ul class="dropdown-menu dropdown-menu-dark">
                            <li><span class="dropdown-item text-muted small">Select a list first</span></li>
                            <li>@Html.ActionLink("List Builder →", "Index", "ListBuilder", Nothing, New With {.class = "dropdown-item"})</li>
                        </ul>
                    </li>

                    <!-- Module C -->
                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">
                            <i class="bi bi-grid-3x3-gap me-1"></i> Views
                        </a>
                        <ul class="dropdown-menu dropdown-menu-dark">
                            <li><span class="dropdown-item text-muted small">Select a list first</span></li>
                            <li>@Html.ActionLink("List Builder →", "Index", "ListBuilder", Nothing, New With {.class = "dropdown-item"})</li>
                        </ul>
                    </li>

                    <!-- Module E -->
                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">
                            <i class="bi bi-diagram-3-fill me-1"></i> Workflows
                        </a>
                        <ul class="dropdown-menu dropdown-menu-dark">
                            <li><span class="dropdown-item text-muted small">Select a list first</span></li>
                            <li>@Html.ActionLink("List Builder →", "Index", "ListBuilder", Nothing, New With {.class = "dropdown-item"})</li>
                        </ul>
                    </li>

                    <!-- Module F -->
                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">
                            <i class="bi bi-shield-lock-fill me-1"></i> Security
                        </a>
                        <ul class="dropdown-menu dropdown-menu-dark">
                            <li>@Html.ActionLink("Roles", "Roles", "Security", Nothing, New With {.class = "dropdown-item"})</li>
                            <li>@Html.ActionLink("Permissions", "Permissions", "Security", Nothing, New With {.class = "dropdown-item"})</li>
                            <li>@Html.ActionLink("User Assignments", "UserRoles", "Security", Nothing, New With {.class = "dropdown-item"})</li>
                        </ul>
                    </li>

                </ul>
                @Html.Partial("_LoginPartial")
            </div>
        </div>
    </nav>

    <div class="container-fluid body-content px-4 py-3">
        @If TempData("SuccessMessage") IsNot Nothing Then
            @Html.Raw($"<div class='alert alert-success alert-dismissible fade show' role='alert'>{TempData("SuccessMessage")}<button type='button' class='btn-close' data-bs-dismiss='alert'></button></div>")
        End If
        @If TempData("ErrorMessage") IsNot Nothing Then
            @Html.Raw($"<div class='alert alert-danger alert-dismissible fade show' role='alert'>{TempData("ErrorMessage")}<button type='button' class='btn-close' data-bs-dismiss='alert'></button></div>")
        End If

        @RenderBody()

        <hr class="mt-4" />
        <footer class="text-center text-muted small py-2">
            &copy; @DateTime.Now.Year — SharePoint-Like Platform | ASP.NET MVC 5 + VB.NET + .NET 4.8
        </footer>
    </div>

    @Scripts.Render("~/bundles/jquery")
    @Scripts.Render("~/bundles/bootstrap")
    @RenderSection("scripts", required:=False)
</body>
</html>
