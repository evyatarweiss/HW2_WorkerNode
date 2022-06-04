using WorkerNode;

var builder = WebApplication.CreateBuilder(args);

string requestQueueUrl_dequeue;
string completedQueueUrl_enqueue;



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

if (!string.IsNullOrEmpty(builder.Configuration["QueueHost"]))
{
    requestQueueUrl_dequeue = builder.Configuration["QueueHost"] + "/Queue/workerQueue/dequeue";
    completedQueueUrl_enqueue = builder.Configuration["QueueHost"] + "/Queue/completedQueue/enqueue";
}
else
{
    requestQueueUrl_dequeue = "https://localhost:7108" + "/Queue/workerQueue/dequeue";
    completedQueueUrl_enqueue = "https://localhost:7108" + "/Queue/completedQueue/enqueue";
}


Console.WriteLine("Before start working");
IWorkerManager workersManager = new WorkerManager(requestQueueUrl_dequeue, completedQueueUrl_enqueue);
workersManager.StartWorking();
Console.WriteLine("After start working");


builder.Services.AddSingleton(_ => workersManager);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


