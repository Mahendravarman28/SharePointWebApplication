Imports SH_WebApplication1.Models.Metadata

Namespace Services

    Public Interface IFormService
        Function GetForms(listId As Integer) As List(Of AppForm)
        Function GetFormById(formId As Integer) As AppForm
        Function GetFormByType(listId As Integer, formType As String) As AppForm
        Function SaveForm(form As AppForm) As AppForm
        Function PublishForm(formId As Integer) As Boolean
        Function DeleteForm(formId As Integer) As Boolean
    End Interface

End Namespace
