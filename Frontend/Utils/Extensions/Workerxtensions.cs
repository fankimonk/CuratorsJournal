using Contracts.Workers;

namespace Frontend.Utils.Extensions
{
    public static class Workerxtensions
    {
        public static string GetFIO(this WorkerResponse worker)
        {
            return worker.LastName + " " + worker.FirstName + " " + worker.MiddleName;
        }
    }
}
