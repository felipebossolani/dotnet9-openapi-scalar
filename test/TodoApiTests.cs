namespace dotnet9_openapi_scalar_test;

using System.Net;
using System.Net.Http.Json;
using Xunit;
using dotnet9_openapi_scalar;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

[TestCaseOrderer("TestOrderExamples.TestCaseOrdering.PriorityOrderer", "TestOrderExamples")]

public class TodoApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TodoApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task AddTodoItem_ReturnsCreatedTodo()
    {
        var todoTitle = "Learn Integration Testing";
        var request = new TodoInsertRequest(todoTitle);
        var response = await _client.PostAsJsonAsync("/todos", request);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var todo = await response.Content.ReadFromJsonAsync<TodoItem>();
        todo.Should().NotBeNull();
        todo.Title.Should().Be(todoTitle);
        todo.IsCompleted.Should().BeFalse();
    }

    [Fact]
    public async Task GetTodoById_ReturnsTodoItem()
    {
        var todoTitle = "Test Get By Id";
        var request = new TodoInsertRequest(todoTitle);
        var addResponse = await _client.PostAsJsonAsync("/todos", request);
        var addedTodo = await addResponse.Content.ReadFromJsonAsync<TodoItem>();

        var getResponse = await _client.GetAsync($"/todos/{addedTodo.Id}");
        getResponse.EnsureSuccessStatusCode();

        var todo = await getResponse.Content.ReadFromJsonAsync<TodoItem>();
        todo.Should().NotBeNull();
        todo.Title.Should().Be(todoTitle);
        todo.IsCompleted.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateTodoItem_ReturnsUpdatedTodo()
    {
        var todoTitle = "Test Update";
        var request = new TodoInsertRequest(todoTitle);
        var addResponse = await _client.PostAsJsonAsync("/todos", request);
        var addedTodo = await addResponse.Content.ReadFromJsonAsync<TodoItem>();

        var updateRequest = new TodoUpdateRequest("Updated Title", true);
        var updateResponse = await _client.PutAsJsonAsync($"/todos/{addedTodo.Id}", updateRequest);
        updateResponse.EnsureSuccessStatusCode();

        var updatedTodo = await updateResponse.Content.ReadFromJsonAsync<TodoItem>();
        updatedTodo.Should().NotBeNull();
        updatedTodo.Title.Should().Be("Updated Title");
        updatedTodo.IsCompleted.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteTodoItem_ReturnsOk()
    {
        var todoTitle = "Test Delete";
        var request = new TodoInsertRequest(todoTitle);
        var addResponse = await _client.PostAsJsonAsync("/todos", request);
        var addedTodo = await addResponse.Content.ReadFromJsonAsync<TodoItem>();

        var deleteResponse = await _client.DeleteAsync($"/todos/{addedTodo.Id}");
        deleteResponse.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync($"/todos/{addedTodo.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
